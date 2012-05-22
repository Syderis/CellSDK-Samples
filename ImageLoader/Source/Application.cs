/*
 * Copyright 2011 Syderis Technologies S.L. All rights reserved.
 * Use is subject to license terms.
 */

#region Using Statements
using Syderis.CellSDK.Core;
using Syderis.CellSDK.Common;
#endregion

namespace ImageLoader
{
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

            //Setup Viewport Manager
            Preferences.ViewportManager.Adjustment = ViewportAdjustment.FIT;
            Preferences.ViewportManager.AlignType = ViewportAlignType.TOPCENTER;
            Preferences.ViewportManager.VirtualWidth = 480;
            Preferences.ViewportManager.VirtualHeight = 800;

            StaticContent.ScreenManager.GoToScreen(new MainScreen());
           
        }

        public override void Exit()
        {
            base.Exit();

            Program.Instance.Exit();
        }

       
    }
}