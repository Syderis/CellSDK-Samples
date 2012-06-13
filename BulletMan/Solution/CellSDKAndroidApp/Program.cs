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

namespace BulletMan
{
    //Uncomment the following block code to enable C2DM push notifications
    /*
    [Application(Label = "CellSDKAndroidApp", Icon = "@drawable/icon")]
    public class CellSDKApplication : Android.App.Application
    {
        /// <summary>
        /// Replace APP_PACKAGE value with your own C2DM package
        /// </summary>
        public const string APP_PACKAGE = "CellSDKAndroidApp.CellSDKAndroidApp";

        public CellSDKApplication(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {
        }
    }

    [BroadcastReceiver(Permission = Syderis.CellSDK.Notifications.C2dm.C2dmClient.GOOGLE_PERMISSION_C2DM_SEND)]
    [IntentFilter(new string[] { Syderis.CellSDK.Notifications.C2dm.C2dmClient.GOOGLE_ACTION_C2DM_INTENT_RECEIVE }, Categories = new string[] { CellSDKApplication.APP_PACKAGE })]
    [IntentFilter(new string[] { Syderis.CellSDK.Notifications.C2dm.C2dmClient.GOOGLE_ACTION_C2DM_INTENT_REGISTRATION }, Categories = new string[] { CellSDKApplication.APP_PACKAGE })]
    public class CustomNotificationBroadcastReceiver : Syderis.CellSDK.Notifications.C2dm.NotificationBroadcastReceiver
    {
    }
    */

    [Activity(Label = "CellSDKAndroidApp", MainLauncher = true, Icon = "@drawable/icon",
        ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation)]
    public class Program : AndroidGameActivity
    {
        /// <summary>
        /// Initial orientation supported.
        /// </summary>
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
