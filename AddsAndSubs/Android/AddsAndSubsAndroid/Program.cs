using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Syderis.CellSDK.Android.Launcher;
using Syderis.CellSDK.Core;

namespace AddsAndSubs
{
    [Activity(Label = "AddsAndSubsAndroid", MainLauncher = true, Icon = "@drawable/icon")]
    public class Program : Activity
    {
        public static Program Instance;
        Kernel view;

        /// <summary>
        /// The main method which loads Application.
        /// </summary>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            view = new Kernel(this);
            SetContentView(view.Window);

            MultitouchStaticContent.SkinXMLFileStream = Assets.Open("Content/Skin/Skin.xml");

            Application application = new Application();
            view.Application = application;
            view.Run();
        }

        public void Exit()
        {
            if (view != null)
            {
                view.Exit();
                this.Finish();
            }
        }
    }
}