/*
 * Copyright 2012 Syderis Technologies S.L. All rights reserved.
 * Use is subject to license terms.
 */

#region Using Statements
using MonoTouch.Foundation;
using MonoTouch.UIKit;

using Syderis.CellSDK.IOS.Launcher;
using Microsoft.Xna.Framework;
#endregion

namespace GamePad
{
	[Register("AppDelegate")]
	class Program : UIApplicationDelegate
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
		
		static void Main (string[] args)
		{
			UIApplication.Main (args, null, "AppDelegate");
		}

		public override void FinishedLaunching (UIApplication app)
		{
			Instance = this;
			Application application = new Application ();
			kernel = new Kernel (application);
			kernel.Run ();
			application.SupportedOrientation = SUPPORTED_ORIENTATION;

		}

		public void Exit ()
		{
			kernel.Exit ();
		}
		
		public override void DidEnterBackground (UIApplication application)
		{
			kernel.OnDeactivated ();
		}
		
		public override void WillEnterForeground (UIApplication application)
		{
			kernel.OnActivated ();
		}
		
		public override void WillTerminate (UIApplication application)
		{
			kernel.OnExiting ();
		}
		
		public override void OnActivated (UIApplication application)
		{
			kernel.OnActivated();
		}
		
		public override void OnResignActivation (UIApplication application)
		{
			kernel.OnDeactivated();
		}
	}
}
