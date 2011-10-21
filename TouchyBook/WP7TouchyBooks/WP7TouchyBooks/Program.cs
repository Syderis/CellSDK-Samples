using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TouchyBooks;
using Syderis.CellSDK.WindowsPhone.Launcher;

namespace WP7TouchyBooks
{
    public class Program : Kernel
    {
        protected override void Initialize()
        {
            MyApplication application = new MyApplication();
            base.Application = application;
            base.Initialize();
        }
    }
}
