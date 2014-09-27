
using WatiN.Core;
using WatiN.Core.Native.Windows;

namespace NinjectWithEF.UnitTests_NUnit.BrowserHelper
{
    internal static class BrowserDemoHelper
    {
        public static void BringToFront(Browser browser)
        {
            browser.ShowWindow(NativeMethods.WindowShowStyle.Minimize);
            browser.ShowWindow(NativeMethods.WindowShowStyle.Maximize);
        }
    }
}
