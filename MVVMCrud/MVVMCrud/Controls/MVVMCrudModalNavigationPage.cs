using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace MVVMCrud.CustomView
{
    public class MVVMCrudModalNavigationPage : Xamarin.Forms.NavigationPage
    {
        public MVVMCrudModalNavigationPage()
        {
            On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.FullScreen);
        }
    }
}
