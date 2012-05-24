/*
 * Copyright 2012 Syderis Technologies S.L. All rights reserved.
 * Use is subject to license terms.
 */

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syderis.CellSDK.Core.Screens;
using Syderis.CellSDK.Core.Controls;
using System.Xml.Serialization;
using Syderis.CellSDK.Core.Storage;
using Syderis.CellSDK.Core.Layouts;
using Syderis.CellSDK.Core.Animations;
using Microsoft.Xna.Framework;
using Syderis.CellSDK.Core.Graphics;
using Syderis.CellSDK.Common;
using Syderis.CellSDK.IO.AccelerometerSystem; 
#endregion

namespace PigeonsAttack
{
    public class MainScreen: Screen
    {
        #region Variables
        private TimeSpan PLAYING_TIME = TimeSpan.FromSeconds(60);
        private const int INTERVAL_BETWEEN_SHITS = 2;

        private bool accelerometerDetected;
        private Sprite lLeftTire;
        private Sprite lRightTire;
        private TimeSpan elapsedGameTime;
        private Sprite lRed;
        private Sprite lAmber;
        private Sprite lGreen;
        private bool isAmber = false, isGreen = false;
        private Random random;
        private TimeSpan elapsedTimeBetweenShits;
        private Animation aShitFalling;
        private Sprite lShit;
        private Vector2[] shitPositions;
#if DEBUG
        private Label lLog;
#endif
        private int impacts;
        private Sprite cCar;
        private Sprite lCar;
        private float maxClamp, minClamp;
        private Label lImpacts;
        private Sprite lImpactsBrand; 
        #endregion

        #region Public Methods
        /// <summary>
        /// Sets the screen up (UI components, multimedia content, etc.)
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // Background
			SetBackground(ResourceManager.CreateImage("Images/background_ios"), Screen.Adjustment.CENTER);
			
            // Sprite sheet image
            SpriteSheet iSpriteSheet = ResourceManager.CreateSpriteSheet("Images/PigeonsAttackSpriteSheet");

            // Pigeons
            Sprite[] lPigeons = new Sprite[5];
            Image iPigeonLookingAtLeft = iSpriteSheet["pigeon_1"];
            Image iPigeonLookingAtRight = iSpriteSheet["pigeon_2"];
            random = new Random();

            lPigeons[0] = new Sprite("pigeon", iPigeonLookingAtLeft);
            lPigeons[1] = new Sprite("pigeon", iPigeonLookingAtLeft);
            lPigeons[2] = new Sprite("pigeon", iPigeonLookingAtRight);
            lPigeons[3] = new Sprite("pigeon", iPigeonLookingAtLeft);
            lPigeons[4] = new Sprite("pigeon", iPigeonLookingAtRight);

            AddComponent(lPigeons[0], 79, 12);
            AddComponent(lPigeons[1], 207, 15);
            AddComponent(lPigeons[2], 345, 16);
            AddComponent(lPigeons[3], 495, 16);
            AddComponent(lPigeons[4], 634, 7);

            // Car 
            cCar = new Sprite("cCar", Image.CreateImage(Color.Transparent, 238, 164));

            // Car itself
            lCar = new Sprite("car", iSpriteSheet["car"]);
            cCar.AddChild(lCar);
            
            // Tires
            Image iTire = iSpriteSheet["steel"];
            lLeftTire = new Sprite("lLeftTire", iTire) { Position = new Vector2(70, 138), Pivot = Vector2.One / 2};
            cCar.AddChild(lLeftTire);

            lRightTire = new Sprite("lRightTire", iTire) { Position = new Vector2(162, 138), Pivot = Vector2.One / 2 };
            cCar.AddChild(lRightTire);
            


            AddComponent(cCar, 11, 480 - 164 - 24);

            if (AccelerometerSensor.Instance.IsConnected)
            {
                accelerometerDetected = true;
                AccelerometerSensor.Instance.Start();
            }

            #region Shits
            Image iShit = iSpriteSheet["shit"];
            lShit = new Sprite("shit", iShit) { Pivot = Vector2.One / 2};
            // The shit will be hidden since the very begining, so doesn't matter where to place it
            AddComponent(lShit, -100, -100);
            lShit.Visible = false;

            shitPositions = new Vector2[5];
            shitPositions[0] = new Vector2(135, 63);
            shitPositions[1] = new Vector2(267, 70);
            shitPositions[2] = new Vector2(367, 70);
            shitPositions[3] = new Vector2(553, 70);
            shitPositions[4] = new Vector2(657, 63);

            elapsedTimeBetweenShits = TimeSpan.Zero;

            // 60 will cause the animation to take 1 s to complete
            aShitFalling = Animation.CreateAnimation(75);
            aShitFalling.AnimationType = AnimationType.Relative;
            aShitFalling.AddKey(new KeyFrame(0, Vector2.Zero));
            aShitFalling.AddKey(new KeyFrame(aShitFalling.NumFrames - 1, new Vector2(0, 400)));
            // Once the shit touches the road it must dissapear
            aShitFalling.EndEvent += delegate { lShit.Visible = false; };
            AddAnimation(aShitFalling);
            #endregion Shits

            // Semaphore lights
            lRed = new Sprite("red", iSpriteSheet["red"]);
            AddComponent(lRed, 716, 187);
            lAmber = new Sprite("amber", iSpriteSheet["amber"]);
            AddComponent(lAmber, 715, 215);
            lGreen = new Sprite("green", iSpriteSheet["green"]);
            AddComponent(lGreen, 717, 241);
            lAmber.Visible = lGreen.Visible = false;

            // This var will hold the playing time
            elapsedGameTime = TimeSpan.Zero;

#if DEBUG
            lLog = new Label("N/A", Color.Black, Color.White);
            AddComponent(lLog, Preferences.ViewportManager.TopLeftAnchor);
#endif

            impacts = 0;
            minClamp = 11;
            // The car won't overpass the semaphore until it turns green
            maxClamp = 545;

            Font fImpacts = ResourceManager.CreateFont("Fonts/spritefont_0_9");
            lImpacts = new Label("0", Color.White, Color.Transparent) { Font = fImpacts};
            AddComponent(lImpacts, 107, 155);
            lImpacts.Visible = false;
            lImpactsBrand = new Sprite("impacts", iSpriteSheet["impacts"]);
            AddComponent(lImpactsBrand, 197, 160);
            lImpactsBrand.Visible = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (accelerometerDetected)
            {
                float offset = -AccelerometerSensor.Instance.Data3.Y;
                
                Vector2 temp = lCar.Position;
                float carPosX = MathHelper.Clamp(temp.X + offset * 25, minClamp, maxClamp);
                temp.X = carPosX;
                cCar.Position = temp;

                //Tire rotation
                lLeftTire.Rotation = carPosX * 0.05f;
                lRightTire.Rotation = carPosX * 0.05f;
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
                maxClamp = Preferences.Height + 200;
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

            if (!lImpacts.Visible && isGreen && lCar.Position.X >= Preferences.ViewportManager.VirtualScreenWidth - lCar.Size.X + 100)
            {
                lImpacts.Text = impacts > 9 ? "9" : impacts.ToString();
                lImpacts.Visible = lImpactsBrand.Visible = true;
            }
        }

        /// <summary>
        /// Pops this screen returning to the previous one, or exiting the app if there is no more left.
        /// This method is called when the hardware back button is pressed (only Windows Phone & Android)
        /// </summary>
        public override void BackButtonPressed()
        {
            base.BackButtonPressed();
        } 
        #endregion
    }
}
