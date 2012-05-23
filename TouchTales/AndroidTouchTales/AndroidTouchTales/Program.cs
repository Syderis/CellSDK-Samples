/*
 * Copyright 2012 Syderis Technologies S.L. All rights reserved.
 * Use is subject to license terms.
 */

#region Using Statements
using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Microsoft.Xna.Framework;

using Syderis.CellSDK.Android.Launcher;
using Syderis.CellSDK.Common;
using Syderis.CellSDK.Core; 
#endregion

namespace TouchyTales
{
    [Activity(Label = "AndroidTochTales", MainLauncher = true, Icon = "@drawable/icon", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation)]
    public class Program : AndroidGameActivity
    {
        private const DisplayOrientation SUPPORTED_ORIENTATION = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;

        public static Program Instance;
        private Kernel kernel;

        /// <summary>
        /// Sets the orientations that the application can handle
        /// </summary>
        /// <param name="supportedOrientation"></param>
        public DisplayOrientation SupportedOrientation
        {
            get
            {
                return this.kernel.SupportedOrientation;
            }

            set
            {
                this.kernel.SupportedOrientation = value;
            }
        }

        /// <summary>
        /// The main method which loads Application.
        /// </summary>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Kernel.Activity = this;

            kernel = new Kernel(this);
            SetContentView(kernel.Window);

            Instance = this;
            Preferences.SkinXMLFileStream = Assets.Open("Content/Skin/Skin.xml");
            Preferences.ApplicationActivity = this;

            Application application = new Application();
            kernel.Application = application;
            application.SupportedOrientation = SUPPORTED_ORIENTATION;
            kernel.Run();
        }

        /// <summary>
        /// Exit Method.
        /// </summary>
        public void Exit()
        {
            Finish();

            kernel.KillApp();
        }

        /// <summary>
        /// The application's activity pauses the execution
        /// </summary>
        protected override void OnPause()
        {
            base.OnPause();

            kernel.OnPause();
        }

        /// <summary>
        /// The application's activity resumes the execution
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();

            kernel.OnResume();
        }
    }
}