using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

using Syderis.CellSDK.IOS.Launcher;

namespace ShareBillAndroid
{
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
		
		public void Exit()
        {
            
        }
    }
}
