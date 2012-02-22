/*
 * Copyright 2012 Syderis Technologies S.L. All rights reserved.
 * Use is subject to license terms.
 */

#region Using Statements
using Syderis.CellSDK.Core; 
#endregion

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

            StaticContent.Graphics.IsFullScreen = true;
            StaticContent.Graphics.ApplyChanges();

            StaticContent.ScreenManager.GoToScreen(new MainScreen());
      
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
