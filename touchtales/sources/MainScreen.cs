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
#endregion

namespace TouchyTales
{
    public class MainScreen: Screen
    {
        #region Consts and statis
        private static string imagePath = "Images/";
        #endregion

        #region Variables
        private Label doll, rope, ball, orangeBall, purpleBall, blueBall, redBall, greenBall, lpipin, train;
        private Animation atrain, aball, arope;
        private StripAnimation anim_pipin, anim_ball;
        private Image itrain_on, itrain_off;
        private Random random;
        #endregion

        #region Events
        /// <summary>
        /// When the ball is touched, it plays a sound and play the animation.
        /// </summary>
        /// <param name="source"></param>
        void ball_Released(Component source)
        {
            AudioLibrary.Instance.Play(AudioLibrary.SBALL2);
            anim_ball.Play();
        }

        /// <summary>
        /// When the bird (pipin) is touched it plays a sound and play the animation.
        /// </summary>
        /// <param name="source"></param>
        void lpipin_Released(Component source)
        {
            AudioLibrary.Instance.Play(AudioLibrary.SPIPIN);
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
                AudioLibrary.Instance.Play(AudioLibrary.SROPE);
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
                AudioLibrary.Instance.Play(AudioLibrary.SDOLL2);
            else
                AudioLibrary.Instance.Play(AudioLibrary.SDOLL1);
        }

        /// <summary>
        /// When a ball is touched, it plays a sound and play the animation.
        /// </summary>
        /// <param name="source"></param>
        void Ball_Released(Component source)
        {
            AudioLibrary.Instance.Play(AudioLibrary.STRAIN);
            aball.Play(source);
        }

        /// <summary>
        /// When the train is touched, it change the train image, plays a sound and play the animation.
        /// </summary>
        /// <param name="source"></param>
        void train_Released(Component source)
        {
            AudioLibrary.Instance.Play(AudioLibrary.STRAIN);
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
            AudioLibrary.Instance.Stop(AudioLibrary.STRAIN);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Sets the screen up (UI components, multimedia content, etc.)
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            random = new Random();

            SetBackground(ResourceManager.CreateImage(imagePath + "bg"), Adjustment.CENTER);

            //Rope
            rope = new Label(ResourceManager.CreateImage(imagePath + "rope"));
            rope.Released += new Component.ComponentEventHandler(rope_Released);
            rope.Draggable = false;
            rope.BringToFront = false;
            AddComponent(rope, 422, 384);

            //Doll
            doll = new Label(ResourceManager.CreateImage(imagePath + "doll"));
            doll.BringToFront = false;
            doll.Draggable = false;
            AddComponent(doll, 446, 223);

            //Ball
            AnimatedImage img = ResourceManager.CreateAnimatedImage(imagePath + "animated_ball");
            anim_ball = new StripAnimation(1505, 687, 215, 229, 19);
            anim_ball.FramesPerSecond = 40;
            anim_ball.IsLooped = false;
            img.AddAnimation("ball", anim_ball);
            ball = new Label(img);
            ball.BringToFront = false;
            ball.Released += new Component.ComponentEventHandler(ball_Released);
            AddComponent(ball, 252, 148);

            //Inked Balls          
            orangeBall = new Label(ResourceManager.CreateImage(imagePath + "yellow_ball"));
            orangeBall.Released += new Component.ComponentEventHandler(Ball_Released);
            orangeBall.Draggable = false;
            AddComponent(orangeBall, -23, 125);

            blueBall = new Label(ResourceManager.CreateImage(imagePath + "blue_ball"));
            blueBall.Released += new Component.ComponentEventHandler(Ball_Released);
            blueBall.Draggable = false;
            AddComponent(blueBall, 77, 57);

            purpleBall = new Label(ResourceManager.CreateImage(imagePath + "purpel_ball"));
            purpleBall.Released += new Component.ComponentEventHandler(Ball_Released);
            purpleBall.Draggable = false;
            AddComponent(purpleBall, 0, 4);

            redBall = new Label(ResourceManager.CreateImage(imagePath + "red_ball"));
            redBall.Released += new Component.ComponentEventHandler(Ball_Released);
            redBall.Draggable = false;
            AddComponent(redBall, 8, 205);

            greenBall = new Label(ResourceManager.CreateImage(imagePath + "green_ball"));
            greenBall.Released += new Component.ComponentEventHandler(Ball_Released);
            greenBall.Draggable = false;
            AddComponent(greenBall, 50, 100);

            //Bird
            AnimatedImage pipin = ResourceManager.CreateAnimatedImage(imagePath + "animated_bird");
            anim_pipin = new StripAnimation(288, 106, 48, 106, 6);
            anim_pipin.IsLooped = false;
            anim_pipin.FramesPerSecond = 4;
            pipin.AddAnimation("bird", anim_pipin);
            lpipin = new Label(pipin);
            lpipin.Draggable = false;
            lpipin.Released += new Component.ComponentEventHandler(lpipin_Released);
            AddComponent(lpipin, 490, 42);

            //Train 
            itrain_off = ResourceManager.CreateImage(imagePath + "train_off");
            itrain_on = ResourceManager.CreateImage(imagePath + "train_on");
            train = new Label(itrain_off);
            train.Released += new Component.ComponentEventHandler(train_Released);
            train.Draggable = false;
            AddComponent(train, 125, 314);

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
            AddAnimation(aball);

            //-> Train
            atrain = Animation.CreateAnimation(250);
            atrain.AnimationType = AnimationType.Relative;
            atrain.AddKey(new KeyFrame(0, Vector2.Zero, 0, 1, 1));
            atrain.AddKey(new KeyFrame(49, new Vector2( (0.3f) * 800, 0.345f * 480), 0, 1.5f, 1));
            atrain.AddKey(new KeyFrame(50, new Vector2(-0.702f * 800, -0.44375f * 480), 0, 0.8f, 1));
            atrain.AddKey(new KeyFrame(150, new Vector2(-0.702f * 800, -0.44375f * 480), 0, 0.8f, 1));
            atrain.AddKey(new KeyFrame(atrain.NumFrames - 1, Vector2.Zero, 0, 1, 1));
            atrain.EndEvent += new Animation.AnimationHandler(atrain_EndEvent);
            AddAnimation(atrain);

            //-> Rope
            arope = Animation.CreateAnimation(75);
            arope.AnimationType = AnimationType.Relative;
            arope.AddKey(new KeyFrame(0, Vector2.Zero));
            arope.AddKey(new KeyFrame(10, new Vector2(-40, 0), true));
            arope.AddKey(new KeyFrame(arope.NumFrames - 1, Vector2.Zero));
            arope.KeyEvent += new Animation.AnimationHandler(arope_KeyEvent);
            AddAnimation(arope);

            //Initialize sound library
            AudioLibrary.Instance.Initialize();

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
