/*
 * Copyright 2011 Syderis Technologies S.L. All rights reserved.
 * Use is subject to license terms.
 * 
 * This sample shows a scene with some object to interact with.
 * 
 */

#region Using Statements
using System;
using Syderis.CellSDK.Core;
using Syderis.CellSDK.Core.Controls;
using Microsoft.Xna.Framework;
using Syderis.CellSDK.Core.Graphics;
using Syderis.CellSDK.Core.Animations;
using Syderis.CellSDK.Common;
#endregion

namespace TouchyTales
{
    public class Application : MobileApplication
    {
        #region Consts and statis
        private static string imagePath = "Images/";
        #endregion

        #region Variables
        private Label doll, rope, ball, orangeBall, purpleBall, blueBall, redBall, greenBall, lpipin, train;
        private Animation atrain, aball, arope;
        private StripAnimation anim_pipin, anim_ball;
        private AudioLibrary audioLibrary;
        private Image itrain_on, itrain_off;
        private Random random;
        #endregion

        #region Public methods
        /// <summary>
        /// Initialization method.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            random = new Random();
            audioLibrary = AudioLibrary.GetInstance();

            SetBackground(Image.CreateImage(imagePath + "bg"), Adjustment.NONE);            

            //Rope
            rope = new Label(Image.CreateImage(imagePath + "rope"));
            rope.Released += new Component.ComponentEventHandler(rope_Released);
            rope.Draggable = false;
            rope.BringToFront = false;
            Add(rope, 0.2f, 0.527f);

            //Doll
            doll = new Label(Image.CreateImage(imagePath + "doll"));
            doll.BringToFront = false;
            doll.Draggable = false;
            Add(doll, 0.535f, 0.557f);

            //Ball
            AnimatedImage img = AnimatedImage.CreateImage(imagePath + "animated_ball");
            anim_ball = new StripAnimation(1505, 687, 215, 229, 19);
            anim_ball.FramesPerSecond = 40;
            anim_ball.IsLooped = false;
            img.AddAnimation("ball", anim_ball);
            ball = new Label(img);
            ball.BringToFront = false;
            ball.Released += new Component.ComponentEventHandler(ball_Released);
            Add(ball, 0.691f, 0.315f);

            //Inked Balls          
            orangeBall = new Label(Image.CreateImage(imagePath + "yellow_ball"));
            orangeBall.Released += new Component.ComponentEventHandler(Ball_Released);
            orangeBall.Draggable = false;
            Add(orangeBall, 0.739f, -0.028f);

            blueBall = new Label(Image.CreateImage(imagePath + "blue_ball"));
            blueBall.Released += new Component.ComponentEventHandler(Ball_Released);
            blueBall.Draggable = false;
            Add(blueBall, 0.881f, 0.096f);

            purpleBall = new Label(Image.CreateImage(imagePath + "purpel_ball"));
            purpleBall.Released += new Component.ComponentEventHandler(Ball_Released);
            purpleBall.Draggable = false;
            Add(purpleBall, 0.991f, 0f);

            redBall = new Label(Image.CreateImage(imagePath + "red_ball"));
            redBall.Released += new Component.ComponentEventHandler(Ball_Released);
            redBall.Draggable = false;
            Add(redBall, 0.572f, 0.01f);

            greenBall = new Label(Image.CreateImage(imagePath + "green_ball"));
            greenBall.Released += new Component.ComponentEventHandler(Ball_Released);
            greenBall.Draggable = false;
            Add(greenBall, 0.791f, 0.0625f);

            //Bird
            AnimatedImage pipin = AnimatedImage.CreateImage(imagePath + "animated_bird");
            anim_pipin = new StripAnimation(288, 106, 48, 106, 6);
            anim_pipin.IsLooped = false;
            anim_pipin.FramesPerSecond = 4;
            pipin.AddAnimation("bird", anim_pipin);
            lpipin = new Label(pipin);
            lpipin.Draggable = false;
            lpipin.Released += new Component.ComponentEventHandler(lpipin_Released);
            Add(lpipin, 0.912f, 0.6125f);

            //Train 
            itrain_off = Image.CreateImage(imagePath + "train_off");
            itrain_on = Image.CreateImage(imagePath + "train_on");
            train = new Label(itrain_off);
            train.Released += new Component.ComponentEventHandler(train_Released);
            train.Draggable = false;
            Add(train, 0.345f, 0.15625f);

            //Animations 
            //- > Ball
            aball = Animation.CreateAnimation(12);
            aball.AnimationType = AnimationType.Relative;
            int quakeoffset = 10;
            aball.AddKey(new KeyFrame(0, Vector2.Zero));
            aball.AddKey(new KeyFrame(3, new Vector2(-quakeoffset, 0)));
            aball.AddKey(new KeyFrame(6, new Vector2(quakeoffset, 0)));
            aball.AddKey(new KeyFrame(9, new Vector2(-quakeoffset, 0)));
            aball.AddKey(new KeyFrame(aball.NumFrames - 1, Vector2.Zero));

            //-> Train
            atrain = Animation.CreateAnimation(250);
            atrain.AnimationType = AnimationType.Relative;
            atrain.AddKey(new KeyFrame(0, Vector2.Zero, 0, 1, 1));
            atrain.AddKey(new KeyFrame(49, new Vector2(TransformXCoordinate(0.781f), TransformYCoordinate(0.345f)), 0, 1.5f, 1));
            atrain.AddKey(new KeyFrame(50, new Vector2(TransformXCoordinate(-1.702f), TransformYCoordinate(-0.44375f)), 0, 0.8f, 1));
            atrain.AddKey(new KeyFrame(150, new Vector2(TransformXCoordinate(-1.702f), TransformYCoordinate(-0.44375f)), 0, 0.8f, 1));
            atrain.AddKey(new KeyFrame(atrain.NumFrames - 1, Vector2.Zero, 0, 1, 1));
            atrain.EndEvent += new Animation.AnimationHandler(atrain_EndEvent);

            //-> Rope
            arope = Animation.CreateAnimation(75);
            arope.AnimationType = AnimationType.Relative;
            arope.AddKey(new KeyFrame(0, Vector2.Zero));
            arope.AddKey(new KeyFrame(10, new Vector2(TransformXCoordinate(-0.0833f), 0), true));
            arope.AddKey(new KeyFrame(arope.NumFrames - 1, Vector2.Zero));
            arope.KeyEvent += new Animation.AnimationHandler(arope_KeyEvent);



        }

        /// <summary>
        /// Update method. It does not do nothing special
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw method. It does not do nothing special
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

        }

        /// <summary>
        /// Adds a component to the screen rotating it and in the correcto position
        /// </summary>
        /// <param name="comp">Component to add</param>
        /// <param name="x">X position expressed in percentage between 0 and 1</param>
        /// <param name="y">Y position expressed in percentage between 0 and 1</param>
        public void Add(Component comp, float x, float y)
        {

            comp.LocalWorld *= Matrix3x3.CreateRotation((float)Math.PI / 2);
            AddComponent(comp, TransformXCoordinate(x),
                                TransformYCoordinate(y));

        }


        /// <summary>
        /// Release all resources  
        /// </summary>
        public override void Dispose()
        {
            audioLibrary.Dispose();
            if (atrain != null)
            {
                atrain.Dispose();
                atrain = null;
            }
            if (aball != null)
            {
                aball.Dispose();
                aball = null;
            }
            if (arope != null)
            {
                arope.Dispose();
                arope = null;
            }
            base.Dispose();
        }

        public override void BackButtonPressed()
        {
            Program.Instance.Exit();
        }

        #endregion
        #region Events

        /// <summary>
        /// When the ball is touched, it plays a sound and play the animation.
        /// </summary>
        /// <param name="source"></param>
        void ball_Released(Component source)
        {
            audioLibrary.Play(AudioLibrary.SBALL2);
            anim_ball.Play();
        }

        /// <summary>
        /// When the bird (pipin) is touched it plays a sound and play the animation.
        /// </summary>
        /// <param name="source"></param>
        void lpipin_Released(Component source)
        {
            audioLibrary.Play(AudioLibrary.SPIPIN);
            anim_pipin.Play();
        }

        /// <summary>
        /// When the rope is touched it plays a sound and play the animation only if the rope animation is not playing.
        /// </summary>
        /// <param name="source"></param>
        void rope_Released(Component source)
        {
            if (arope.State != AnimationState.Playing)
            {
                audioLibrary.Play(AudioLibrary.SROPE);
                arope.Play(rope);
            }
        }

        /// <summary>
        /// When the rope animation throw the keyevent, a randomized sound is played.
        /// </summary>
        /// <param name="animation"></param>
        void arope_KeyEvent(Animation animation)
        {
            int r = random.Next(0, 100);
            if (r < 51)
                audioLibrary.Play(AudioLibrary.SDOLL2);
            else
                audioLibrary.Play(AudioLibrary.SDOLL1);
        }

        /// <summary>
        /// When a ball is touched, it plays a sound and play the animation.
        /// </summary>
        /// <param name="source"></param>
        void Ball_Released(Component source)
        {
            audioLibrary.Play(AudioLibrary.SBALL);
            aball.Play(source);
        }

        /// <summary>
        /// When the train is touched, it change the train image, plays a sound and play the animation.
        /// </summary>
        /// <param name="source"></param>
        void train_Released(Component source)
        {
            audioLibrary.Play(AudioLibrary.STRAIN);
            train.Image = itrain_on;
            atrain.Play(train);
        }

        /// <summary>
        /// When the train animation ends, the train image is changed and stop the sound.
        /// </summary>
        /// <param name="animation"></param>
        void atrain_EndEvent(Animation animation)
        {
            train.Image = itrain_off;
            audioLibrary.Stop(AudioLibrary.STRAIN);
        }

       
        #endregion

        #region Private methods
        /// <summary>
        /// Transforms a float x coordinate between 0 and 1 to the correspondent pixel position in the screen
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private float TransformXCoordinate(float x)
        {
            return x * Preferences.Width;
        }

        /// <summary>
        /// Transforms a float y coordinate between 0 and 1 to the correspondent pixel position in the screen
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        private float TransformYCoordinate(float y)
        {
            return y * Preferences.Height;
        }
        #endregion
    }
}
