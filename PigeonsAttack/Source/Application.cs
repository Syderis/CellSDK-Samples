#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Syderis.CellSDK.Core;
using Syderis.CellSDK.Core.Animations;
using Syderis.CellSDK.Core.Controls;
using Syderis.CellSDK.Core.Graphics;
using Syderis.CellSDK.Core.Layouts;
using Syderis.CellSDK.IO.AccelerometerSystem;
#endregion

namespace PigeonsAttack
{
    class Application : MobileApplication
    {
        private TimeSpan PLAYING_TIME = TimeSpan.FromSeconds(60);
        private const int INTERVAL_BETWEEN_SHITS = 2;

        private bool accelerometerDetected;
        private Container<CoordLayout> cCar;
        private float offset;
        private Label lLeftTire;
        private Label lRightTire;
        private TimeSpan elapsedGameTime;
        private Label lRed;
        private Label lAmber;
        private Label lGreen;
        private bool isAmber = false, isGreen = false;
        private Random random;
        private TimeSpan elapsedTimeBetweenShits;
        private Animation aShitFalling;
        private Label lShit;
        private Vector2[] shitPositions;
#if DEBUG
        private  Label lLog;
#endif
        private int impacts;
        private Label lCar;
        private float maxClamp, minClamp;
        private Label lImpacts;
        private Label lImpactsBrand;

        /// <summary>
        /// The main method for loading controls and resources.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            IsTouchVisible = false;

            // Background
            Label lBackground = new Label(Image.CreateImage("Images/background_ios")) 
            { 
                Pivot = Vector2.One / 2,
                BringToFront = false
            };
            AddComponent(lBackground, Width / 2, Height / 2);

            // Sprite sheet image
            Image iSpriteSheet = Image.CreateImage("Images/spritesheet");

            // Pigeons
            Label[] lPigeons = new Label[5];
            Image iPigeonLookingAtLeft = iSpriteSheet.SubImage(0, 240, 67, 79);
            Image iPigeonLookingAtRight = iSpriteSheet.SubImage(0, 240, 67, 79);
            iPigeonLookingAtRight.Effect = Image.EffectType.FLIP_VERTICAL;
            random = new Random();

            //for (int i = 0; i < lPigeons.Length; i++)
            //    lPigeons[i] = new Label(random.Next(0, 2) == 0 ? iPigeonLookingAtLeft : iPigeonLookingAtRight);

            lPigeons[0] = new Label(iPigeonLookingAtLeft);
            lPigeons[1] = new Label(iPigeonLookingAtLeft);
            lPigeons[2] = new Label(iPigeonLookingAtRight);
            lPigeons[3] = new Label(iPigeonLookingAtLeft);
            lPigeons[4] = new Label(iPigeonLookingAtRight);

            AddComponentDeviceAgnostic(lPigeons[0], 401, 79);
            AddComponentDeviceAgnostic(lPigeons[1], 398, 207);
            AddComponentDeviceAgnostic(lPigeons[2], 397, 345);
            AddComponentDeviceAgnostic(lPigeons[3], 397, 495);
            AddComponentDeviceAgnostic(lPigeons[4], 406, 634);

            // Car component
            cCar = new Container<CoordLayout>(new CoordLayout())
            {
                BackgroundColor = Color.Transparent,
                BringToFront = false
            };
            // Car itself
            lCar = new Label(iSpriteSheet.SubImage(0, 0, 164, 238));
            cCar.Layout.AddComponent(lCar, 0, 0);
            // Tires
            Image iTire = iSpriteSheet.SubImage(69, 240, 52, 51);
            lLeftTire = new Label(iTire);
            cCar.Layout.AddComponent(lLeftTire, 0, 44);
            lRightTire = new Label(iTire);
            cCar.Layout.AddComponent(lRightTire, 0, 137);
            AddComponentDeviceAgnostic(cCar, 24, 11);

            if (AccelerometerSensor.Instance.IsConnected)
            {
                accelerometerDetected = true;
                AccelerometerSensor.Instance.Start();
            }

            #region Shits
            Image iShit = iSpriteSheet.SubImage(0, 321, 44, 23);
            lShit = new Label(iShit);
            // The shit will be hidden since the very begining, so doesn't matter where to place it
            AddComponent(lShit, -100, -100);
            lShit.Visible = false;

            shitPositions = new Vector2[5];
            shitPositions[0] = DeviceAgnosticPosition(390, 124);
            shitPositions[2] = DeviceAgnosticPosition(390, 356);
            shitPositions[3] = DeviceAgnosticPosition(389, 540);
            shitPositions[4] = DeviceAgnosticPosition(403, 647);

            elapsedTimeBetweenShits = TimeSpan.Zero;

            // 60 will cause the animation to take 1 s to complete
            aShitFalling = Animation.CreateAnimation(75);
            aShitFalling.AnimationType = AnimationType.Relative;
            aShitFalling.AddKey(new KeyFrame(0, Vector2.Zero));
            aShitFalling.AddKey(new KeyFrame(aShitFalling.NumFrames - 1, new Vector2(-400, 0)));
            // Once the shit touches the road it must dissapear
            aShitFalling.EndEvent += delegate { lShit.Visible = false; };
            #endregion Shits

            // Semaphore lights
            lRed = new Label(iSpriteSheet.SubImage(0, 346, 28, 32));
            AddComponentDeviceAgnostic(lRed, 265, 716);
            lAmber = new Label(iSpriteSheet.SubImage(123, 240, 27, 32));
            AddComponentDeviceAgnostic(lAmber, 238, 715);
            lGreen = new Label(iSpriteSheet.SubImage(69, 293, 27, 33));
            AddComponentDeviceAgnostic(lGreen, 212, 717);
            lAmber.Visible = lGreen.Visible = false;

            // This var will hold the playing time
            elapsedGameTime = TimeSpan.Zero;

#if DEBUG
            lLog = new Label("N/A", Color.Black, Color.White) { Rotation = MathHelper.PiOver2 };
            AddComponent(lLog, Width, 0);
#endif

            impacts = 0;
			minClamp = DeviceAgnosticY(11);
            // The car won't overpass the semaphore until it turns green
            maxClamp = DeviceAgnosticY(545);

            Font fImpacts = Font.CreateFont("Fonts/spritefont_0_9");
            lImpacts = new Label("0", Color.White, Color.Transparent) { Font = fImpacts, Rotation = MathHelper.PiOver2 };
            AddComponentDeviceAgnostic(lImpacts, 312, 107);
            lImpacts.Visible = false;
            lImpactsBrand = new Label(iSpriteSheet.SubImage(166, 0, 130, 492));
            AddComponentDeviceAgnostic(lImpactsBrand, 175, 201);
            lImpactsBrand.Visible = false;
        }
		
		private void AddComponentDeviceAgnostic(Component c, float x, float y)
		{
#if WINDOWS_PHONE || ANDROID
			AddComponent(c, x, y);
#else
			AddComponent(c, DeviceAgnosticX(x), DeviceAgnosticY(y));
#endif
		}
		
		private Vector2 DeviceAgnosticPosition(float x, float y)
		{
#if WINDOWS_PHONE || ANDROID
			return new Vector2(x, y);
#else
			return new Vector2(DeviceAgnosticX(x), DeviceAgnosticY(y));
#endif
		}
		
		private float DeviceAgnosticX(float x)
		{
#if WINDOWS_PHONE || ANDROID
			return x;
#else
			return x + 81;
#endif
		}
		
		private float DeviceAgnosticY(float y)
		{
#if WINDOWS_PHONE
			return y;
#else
			return y + 79;
#endif
		}

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (accelerometerDetected)
            {
                offset = -AccelerometerSensor.Instance.Data3.Y;
                Vector2 temp = cCar.Position;
                temp.Y = MathHelper.Clamp(temp.Y + offset * 25, minClamp, maxClamp);
                cCar.Position = temp;

                //if (offset >= 0)
                //    lLeftTire.Rotation = lRightTire.Rotation += .1f;
                //else
                //    lLeftTire.Rotation = lRightTire.Rotation -= .1f;
            }

            elapsedGameTime += gameTime.ElapsedGameTime;

            // Amber
            if (!isAmber && elapsedGameTime >= PLAYING_TIME - TimeSpan.FromSeconds(5))
            {
                lRed.Visible = false;
                lAmber.Visible = true;
                isAmber = true;
            } 
            // Green
            else if (!isGreen && elapsedGameTime >= PLAYING_TIME)
            {
                lAmber.Visible = false;
                lGreen.Visible = true;
                isGreen = true;
                // This will allow the car to exit through the right side of the screen
                maxClamp = DeviceAgnosticY(1000);
            }

            #region Shits
            if ((elapsedTimeBetweenShits += gameTime.ElapsedGameTime) >= TimeSpan.FromSeconds(INTERVAL_BETWEEN_SHITS))
            {
                elapsedTimeBetweenShits = TimeSpan.Zero;
                lShit.Position = shitPositions[random.Next(0, shitPositions.Length)];
                lShit.Visible = true;
                aShitFalling.Play(lShit);
            }
            #endregion Shits

            // TODO Should Intersects() check for visibility?
            if (lShit.Visible && lCar.Intersects(lShit))
            { 
                impacts++;
#if DEBUG
                lLog.Text = impacts.ToString();
#endif
                aShitFalling.Stop();
            }

            if (!lImpacts.Visible && isGreen && lCar.Position.Y >= Height + 100)
            {
                lImpacts.Text = impacts > 9 ? "9" : impacts.ToString();
                lImpacts.Visible = lImpactsBrand.Visible = true;
            }
        }

        public override void BackButtonPressed()
        {
            Program.Instance.Exit();
        }
    }
}
