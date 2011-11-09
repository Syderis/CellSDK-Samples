/*
 * Copyright 2011 Syderis Technologies S.L. All rights reserved.
 * Use is subject to license terms.
 */
using Microsoft.Xna.Framework.Content;
using Syderis.CellSDK.Core.Graphics;
using Syderis.CellSDK.Core.Controls;
using Syderis.CellSDK.Core;

namespace ScreenManager
{
    /// <summary>
    /// Represents the options screen.
    /// </summary>
    class OptionsScreen:IScreen
    {

        #region Variables

        private ScreenManager manager;
        private ContentManager content;
        private Image iBackground, iBack, iBack_Pressed;
        private Label lblBackground;
        private Button bBack;

        #endregion

        public OptionsScreen(ScreenManager manager)
        {
            this.manager = manager;
            content = new ContentManager(MultitouchStaticContent.Services, MultitouchStaticContent.Content.RootDirectory);
        }

        #region IScreen Members

        public void LoadScreen()
        {
            //Load Images
            iBackground = Image.CreateImage("Options/background_options", ref content);
            iBack = Image.CreateImage("Options/bt_back", ref content);
            iBack_Pressed = Image.CreateImage("Options/bt_back_pressed", ref content);

            //Sets the background:
            lblBackground = new Label(iBackground);
            lblBackground.BringToFront = false;

            //CreateButton
            bBack = new Button(iBack, iBack_Pressed);
            bBack.Released += new Component.ComponentEventHandler(bBack_Released);

            //Add components to the screen
            manager.AddComponent(lblBackground, 0, 0);
            manager.AddComponent(bBack, 105, 97);
        }



        public void ClearScreen()
        {
            manager.RemoveComponent(lblBackground);
            manager.RemoveComponent(bBack);
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
            if (content != null)
                content.Unload();
            content = null;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Loads the initial screen.
        /// </summary>
        /// <param name="source"></param>
        private void bBack_Released(Component source)
        {
            manager.GoToScreen(ScreenManager.Screens.INIT);
        }
        #endregion
    }
}
