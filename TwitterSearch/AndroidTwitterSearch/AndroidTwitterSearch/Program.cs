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

namespace TwitterSearch
{
    [Activity(Label = "AndroidTwitterSearch", MainLauncher = true, Icon = "@drawable/icon")]
    public class Program : AndroidGameActivity
    {
        public static Program Instance;

        /// <summary>
        /// The main method which loads Application.
        /// </summary>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Kernel.Activity = this;

            Kernel view = new Kernel(this);
            SetContentView(view.Window);

            Instance = this;
            Preferences.SkinXMLFileStream = Assets.Open("Content/Skin/Skin.xml");
            Preferences.ApplicationActivity = this;

            Application application = new Application();
            view.Application = application;
            view.Run();
        }

        /// <summary>
        /// Exit Method.
        /// </summary>
        public void Exit()
        {
            Finish();
        }
    }
}