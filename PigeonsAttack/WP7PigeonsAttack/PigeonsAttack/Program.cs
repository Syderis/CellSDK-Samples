/*
 * Copyright 2012 Syderis Technologies S.L. All rights reserved.
 * Use is subject to license terms.
 */

#region Using Statements
using System;

using Syderis.CellSDK.WindowsPhone.Launcher;
using Microsoft.Xna.Framework;
#endregion

namespace PigeonsAttack
{
    public class Program : Kernel
    {
        private const DisplayOrientation SUPPORTED_ORIENTATION = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        protected override void Initialize()
        {
            Application application = new Application();
            FramesPerSecond = 50;
            base.Application = application;
            application.SupportedOrientation = SUPPORTED_ORIENTATION;
            base.Initialize();
        }
    }
}