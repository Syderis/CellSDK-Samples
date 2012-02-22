/*
 * Copyright 2012 Syderis Technologies S.L. All rights reserved.
 * Use is subject to license terms.
 */

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Syderis.CellSDK.Core;
using Syderis.CellSDK.Core.Animations;
using Syderis.CellSDK.Core.Controls;
using Syderis.CellSDK.Core.Graphics;
#endregion 

namespace AddsAndSubs
{
    class Application : MobileApplication
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

        public override void Exit()
        {
            base.Exit();

            Program.Instance.Exit();
        }
    }
       
}
