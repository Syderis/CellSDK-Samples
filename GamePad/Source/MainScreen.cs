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

namespace GamePad
{
    public class MainScreen: Screen
    {
        #region Variables
        private ThumbStick thumbStick;
        private Sprite lMoi;
        private AnimatedImage aiMoi;
        private MoiAnimationStates moiAnimationPreviousState = MoiAnimationStates.FrontRight;
        private enum MoiAnimationStates
        {
            RearRight, // Clockwise
            FrontRight,
            FrontLeft,
            RearLeft
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Sets the screen up (UI components, multimedia content, etc.)
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            SetBackground(ResourceManager.CreateImage("Background"), Adjustment.CENTER);

            #region Moi
            aiMoi = ResourceManager.CreateAnimatedImage("MoiWalkingStripAnimation");

            int width = aiMoi.Width;
            int height = aiMoi.Height;

            StripAnimation saMoiWalkingFrontRight = new StripAnimation(width, height, width / 4, height / 4, 4);
            aiMoi.AddAnimation("MoiWalkingFrontRight", saMoiWalkingFrontRight);
            StripAnimation saMoiWalkingFrontLeft = new StripAnimation(width, height, width / 4, height / 4, 4, 0, height / 4);
            aiMoi.AddAnimation("MoiWalkingFrontLeft", saMoiWalkingFrontLeft);
            StripAnimation saMoiWalkingRearRight = new StripAnimation(width, height, width / 4, height / 4, 4, 0, height / 4 * 2);
            aiMoi.AddAnimation("MoiWalkingRearRight", saMoiWalkingRearRight);
            StripAnimation saMoiWalkingRearLeft = new StripAnimation(width, height, width / 4, height / 4, 4, 0, height / 4 * 3);
            aiMoi.AddAnimation("MoiWalkingRearLeft", saMoiWalkingRearLeft);
            lMoi = new Sprite("Moi", aiMoi) { Pivot = Vector2.One / 2, BringToFront = false };
            AddComponent(lMoi, Preferences.ViewportManager.MiddleCenterAnchor);
            #endregion Moi

            #region Game pad
            
            thumbStick = new ThumbStick();
            thumbStick.Pivot = new Vector2(0, 1);
            Vector2 margin = new Vector2(5, -5);
            AddComponent(thumbStick, Preferences.ViewportManager.BottomLeftAnchor + margin);

            Buttons buttons = new Buttons();
            margin = new Vector2(-5, -5) - buttons.Size;
            AddComponent(buttons, Preferences.ViewportManager.BottomRightAnchor + margin);
            #endregion Game pad

        }

        public override void Update(GameTime gameTime)
        {

            #region Moi position
            Vector2 temp = lMoi.Position;

            if (thumbStick.X > 0)
                temp.X += 3;

            if (thumbStick.X < 0)
                temp.X -= 3;

            if (thumbStick.Y > 0)
                temp.Y += 3;

            if (thumbStick.Y < 0)
                temp.Y -= 3;

            lMoi.Position = temp;
            #endregion Moi position

            // TODO: Let just vertical and horizontal movement (X = 0 and Y <> 0, or X <> 0 and Y = 0)
            #region Moi animation
            MoiAnimationStates current = MoiAnimationStates.FrontRight;

            if (thumbStick.X == 0 && thumbStick.Y == 0)
                aiMoi.Stop();
            else
            {
                if (thumbStick.X > 0 && thumbStick.Y > 0)
                    current = MoiAnimationStates.RearRight;
                else if (thumbStick.X > 0 && thumbStick.Y < 0)
                    current = MoiAnimationStates.RearLeft;
                else if (thumbStick.X < 0 && thumbStick.Y < 0)
                    current = MoiAnimationStates.FrontLeft;
                else if (thumbStick.X < 0 && thumbStick.Y > 0)
                    current = MoiAnimationStates.FrontRight;

                if (current != moiAnimationPreviousState)
                {
                    string key;

                    switch (current)
                    {
                        case MoiAnimationStates.RearRight:
                            key = "MoiWalkingRearRight";
                            break;
                        case MoiAnimationStates.FrontRight:
                            key = "MoiWalkingFrontRight";
                            break;
                        case MoiAnimationStates.FrontLeft:
                            key = "MoiWalkingFrontLeft";
                            break;
                        case MoiAnimationStates.RearLeft:
                            key = "MoiWalkingRearLeft";
                            break;
                        default:
                            // This will never happen...
                            key = "MoiWalkingFrontRight";
                            break;
                    }

                    aiMoi.CurrentAnimationKey = key;
                    aiMoi.Play(true);
                }

                moiAnimationPreviousState = current;
            }
            #endregion Moi animation

            base.Update(gameTime);
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

        #region Private Methods
        /// <summary>
        /// Adds a component at a relative position expressed on percentages from the top-left corner of the screen
        /// </summary>
        /// <param name="c"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void AddComponentPercentage(Component c, float x, float y)
        {
            AddComponent(c, x * Preferences.Width, y * Preferences.Height);
        }
        #endregion
    }
}
