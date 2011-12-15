/*
 * Copyright 2011 Syderis Technologies S.L. All rights reserved.
 * Use is subject to license terms.
 */

using Microsoft.Xna.Framework.Content;
using Syderis.CellSDK.Core;
using Syderis.CellSDK.Core.Graphics;
using Syderis.CellSDK.Core.Controls;

namespace ScreenManager
{
    /// <summary>
    /// Represents the init screen
    /// </summary>
    class InitScreen:IScreen
    {
        #region Variables

        private ScreenManager manager;
        private ContentManager content;
        private Image iBackground, iStart, iStart_pressed, iOptions, iOptions_pressed, iExit, iExit_pressed;
        private Button bStart, bOptions, bExit;

        #endregion

        public InitScreen(ScreenManager manager)
        {
            this.manager = manager;
            content = new ContentManager(StaticContent.Services, StaticContent.Content.RootDirectory);
        }
        #region IScreen Members

        public void LoadScreen()
        {
            //Load images
            iBackground= Image.CreateImage("title/background_titlepage", ref content);
            iStart = Image.CreateImage("title/bt_start", ref content);
            iStart_pressed = Image.CreateImage("title/bt_start_pressed", ref content);
            iOptions = Image.CreateImage("title/bt_options", ref content);
            iOptions_pressed = Image.CreateImage("title/bt_options_pressed", ref content);
            iExit = Image.CreateImage("title/bt_exit", ref content);
            iExit_pressed = Image.CreateImage("title/bt_exit_pressed", ref content);

            //Sets the background:
            manager.App.SetBackground(iBackground, MobileApplication.Adjustment.CENTER);

            //Create the buttons
            bStart = new Button(iStart, iStart_pressed);
            bStart.Released += new Component.ComponentEventHandler(bStart_Released);
            bOptions = new Button(iOptions, iOptions_pressed);
            bOptions.Released += new Component.ComponentEventHandler(bOptions_Released);
            bExit = new Button(iExit, iExit_pressed);
            bExit.Released += new Component.ComponentEventHandler(bExit_Released);

            //Add components to the screen            
            manager.AddComponent(bStart, 250, 166);
            manager.AddComponent(bOptions, 178, 99);
            manager.AddComponent(bExit, 104, 232);

        }


        /// <summary>
        /// Remove components and unload resources
        /// </summary>
        public void ClearScreen()
        {
            manager.RemoveComponent(bStart);
            manager.RemoveComponent(bOptions);
            manager.RemoveComponent(bExit);            
            content.Unload();
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Disposes the object and free the resources
        /// </summary>
        public void Dispose()
        {
            if(content!=null)
                content.Unload();
            content = null;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Exit the application
        /// </summary>
        /// <param name="source"></param>
        private void bExit_Released(Component source)
        {
            manager.BackButtonPressed();
        }

        /// <summary>
        /// Go to options screen.
        /// </summary>
        /// <param name="source"></param>
        private void bOptions_Released(Component source)
        {
            manager.GoToScreen(ScreenManager.Screens.OPTIONS);
        }

        /// <summary>
        /// Go to level screen
        /// </summary>
        /// <param name="source"></param>
        private void bStart_Released(Component source)
        {
            manager.GoToScreen(ScreenManager.Screens.LEVEL);
        }

        #endregion
    }
}
