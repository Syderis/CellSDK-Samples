/*
 * Copyright 2011 Syderis Technologies S.L. All rights reserved.
 * Use is subject to license terms.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Syderis.CellSDK.Core;
using Syderis.CellSDK.Core.Controls;

namespace ScreenManager
{
    /// <summary>
    /// Application class
    /// </summary>
    class Application : MultitouchApplication
    {
        #region Variables
        ScreenManager screenManager; //ScreenManager Instance
        #endregion

        #region Public Methods
        /// <summary>
        /// The main method for loading controls and resources.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        //Instantiates the screen manager
            screenManager = new ScreenManager(this);

        }

        /// <summary>
        /// Calls to screenmanager update method.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            screenManager.Update(gameTime);
        }

        /// <summary>
        /// Release the resources from screen manager.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            screenManager.Dispose();
        }

        /// <summary>
        /// Exits the application.
        /// </summary>
        public override void BackButtonPressed()
        {
            screenManager.BackButtonPressed();
        }

        #endregion
    }
}
