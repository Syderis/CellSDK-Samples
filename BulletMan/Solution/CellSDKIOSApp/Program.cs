using MonoTouch.Foundation;
using MonoTouch.UIKit;

using Microsoft.Xna.Framework;

using Syderis.CellSDK.IOS.Launcher;

namespace BulletMan
{
	[Register("AppDelegate")]
	class Program : UIApplicationDelegate
	{
		/// <summary>
		/// Initial orientation supported.
		/// </summary>
		private const DisplayOrientation SUPPORTED_ORIENTATION = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
		public static Program Instance;
		Application application;
		private Kernel kernel;
		
		/// <summary>
		/// Sets the orientations that the application can handle
		/// </summary>
		/// <param name="supportedOrientation"></param>
		public DisplayOrientation SupportedOrientation {
			get {
				return this.kernel.SupportedOrientation;
			}   

			set {
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
			application = new Application ();
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
		
		public override void OnResignActivation (UIApplication application)
		{
			kernel.OnResignActivation ();
		}
		
		public override void RegisteredForRemoteNotifications (UIApplication uiApplication, NSData deviceToken)
		{
			application.RegisteredForRemoteNotifications (deviceToken);
		}
		
		public override void ReceivedRemoteNotification (UIApplication uiApplication, NSDictionary userInfo)
		{
			application.ReceivedRemoteNotification (userInfo);
		}
		
		public override void FailedToRegisterForRemoteNotifications (UIApplication uiApplication, NSError error)
		{
			application.FailedToRegisterForRemoteNotifications (error);
		}
	}
}
