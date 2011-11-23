using MonoTouch.Foundation;
using MonoTouch.UIKit;

using Syderis.CellSDK.IOS.Launcher;

namespace ScreenManager
{
	[Register("AppDelegate")]
	class Program : UIApplicationDelegate
	{
		public static Program Instance;

		static void Main (string[] args)
		{
			UIApplication.Main (args, null, "AppDelegate");
		}

		public override void FinishedLaunching (UIApplication app)
		{
			Instance=this;
			Application application = new Application ();
			Kernel kernel = new Kernel (application);
			kernel.Run ();
		}

		public void Exit()
		{
			Instance.Exit();
		}
	}
}
