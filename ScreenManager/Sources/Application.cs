/*
 * Copyright 2012 Syderis Technologies S.L. All rights reserved.
 * Use is subject to license terms.
 */

using Syderis.CellSDK.Core;

namespace ScreenManager
{
    /// <summary>
    /// Application class
    /// </summary>
    public class Application : MobileApplication
    {

        /// <summary>
        /// The main method for loading controls and resources.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
      
        }
     
        /// <summary>
        /// Exits the application.
        /// </summary>
        public override void Exit()
        {
            base.Exit();

            Program.Instance.Exit();
        }

    }
}
