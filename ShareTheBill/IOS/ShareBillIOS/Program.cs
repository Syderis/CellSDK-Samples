using System;
using Syderis.CellSDK.Core;

#if WINDOWS_PHONE
using NUIFrameworkPhoneLauncher;
#elif ANDROID
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Launcher.Android;
#endif
#if IPHONE
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Syderis.CellSDK.IOS.Launcher;
#endif

namespace ShareBillAndroid
{

#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
#if WINDOWS_PHONE
   public class Program : Kernel
    {
       protected override void Initialize()
       {
           Application application = new Application();
           base.Application = application;
           base.Initialize();
       }
    }
#endif
#if ANDROID
    [Activity(Label = "ShareTheBill", MainLauncher = true, Icon = "@drawable/icon")]
    public class Program : Activity
    {
        public static Program Instance;
        Kernel view;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Instance = this;

            view = new Kernel(this);

            SetContentView(view.Window);

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
#endif
#if IPHONE
    [Register("AppDelegate")]
    public class Program : UIApplicationDelegate
    {
        public static Program Instance;
        
	public override void FinishedLaunching (UIApplication app)
	{
            Instance = this;
            Application application = new Application ();
            Kernel kernel = new Kernel (application);
            kernel.Run ();
	}
        
	static void Main (string[] args)
        {
            UIApplication.Main (args, null, "AppDelegate");
        }
    }
#endif
}
