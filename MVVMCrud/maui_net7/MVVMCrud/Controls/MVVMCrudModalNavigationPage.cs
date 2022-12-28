
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

namespace MVVMCrud.CustomView
{
    public class MVVMCrudModalNavigationPage : Microsoft.Maui.Controls.NavigationPage
    {
        public MVVMCrudModalNavigationPage()
        {
            On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.FullScreen);
        }
    }
}
