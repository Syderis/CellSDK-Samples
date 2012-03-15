/*
 * Copyright 2012 Syderis Technologies S.L. All rights reserved.
 * Use is subject to license terms.
 */

#region Using Statements
using Android.App;
using Android.OS;
using Microsoft.Xna.Framework;
using Syderis.CellSDK.Android.Launcher;
using Syderis.CellSDK.Common;
#endregion 

namespace AddsAndSubs
{
    [Activity(Label = "AddsAndSubsAndroid", MainLauncher = true, Icon = "@drawable/icon")]
    public class Program : AndroidGameActivity
    {
        public static Program Instance;
        private Kernel kernel;

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
            Syderis.CellSDK.Common.Preferences.SkinXMLFileStream = Assets.Open("Content/Skin/Skin.xml");
            Syderis.CellSDK.Common.Preferences.ApplicationActivity = this;



            Application application = new Application();
            kernel.Application = application;
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