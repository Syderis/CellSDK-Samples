using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Syderis.CellSDK.Android.Launcher;
using Syderis.CellSDK.Core;
using TouchyBooks;

namespace AndroidTouchyBooks
{
    [Activity(Label = "AndroidTouchyBooks", MainLauncher = true, Icon = "@drawable/icon")]
    public class Program : Activity
    {
        /// <summary>
        /// The main method which loads Application.
        /// </summary>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Kernel view = new Kernel(this);
            SetContentView(view.Window);

            MultitouchStaticContent.SkinXMLFileStream = Assets.Open("Content/Skin/Skin.xml");

            MyApplication application = new MyApplication();
            view.Application = application;
            view.Run();
        }
    }
}