/*
 * Copyright 2012 Syderis Technologies S.L. All rights reserved.
 * Use is subject to license terms.
 */

#region Using Statements
using Syderis.CellSDK.Core.Screens;
using Syderis.CellSDK.Core.Graphics;
using Syderis.CellSDK.Core.Controls;
using Syderis.CellSDK.Core; 
#endregion

namespace ScreenManager
{
    public class MainScreen: AdjustedScreen
    {

        private Image iBackground, iStart, iStart_pressed, iOptions, iOptions_pressed, iExit, iExit_pressed;
        private Button bStart, bOptions, bExit;

        public override void Initialize()
        {
            base.Initialize();

            //Load images
            iBackground = ResourceManager.CreateImage("title/background_titlepage");
            iStart = ResourceManager.CreateImage("title/bt_start");
            iStart_pressed = ResourceManager.CreateImage("title/bt_start_pressed");
            iOptions = ResourceManager.CreateImage("title/bt_options");
            iOptions_pressed = ResourceManager.CreateImage("title/bt_options_pressed");
            iExit = ResourceManager.CreateImage("title/bt_exit");
            iExit_pressed = ResourceManager.CreateImage("title/bt_exit_pressed");

            //Sets the background:            
            SetBackground(iBackground, Adjustment.CENTER);

            //Create the buttons
            bStart = new Button(iStart, iStart_pressed);
            bStart.Released += new Component.ComponentEventHandler(bStart_Released);
            bOptions = new Button(iOptions, iOptions_pressed);
            bOptions.Released += new Component.ComponentEventHandler(bOptions_Released);
            bExit = new Button(iExit, iExit_pressed);
            bExit.Released += new Component.ComponentEventHandler(bExit_Released);

            //Add components to the screen            
            AddComponent(bStart, 170, 86);
            AddComponent(bOptions, 98, 19);
            AddComponent(bExit, 24, 152);
        }

        #region Events

        /// <summary>
        /// Exit the application
        /// </summary>
        /// <param name="source"></param>
        private void bExit_Released(Component source)
        {
            BackButtonPressed();
        }

        /// <summary>
        /// Go to options screen.
        /// </summary>
        /// <param name="source"></param>
        private void bOptions_Released(Component source)
        {
            StaticContent.ScreenManager.PushScreen(new OptionsScreen(), StaticContent.TransitionFactory.FadeBlackTransition);
        }

        /// <summary>
        /// Go to level screen
        /// </summary>
        /// <param name="source"></param>
        private void bStart_Released(Component source)
        {
            StaticContent.ScreenManager.PushScreen(new LevelScreen(), StaticContent.TransitionFactory.FadeBlackTransition);
        }

        #endregion

        public override void BackButtonPressed()
        {
            base.BackButtonPressed();
        }
    }
}
