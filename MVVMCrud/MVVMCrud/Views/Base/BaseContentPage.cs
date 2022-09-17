using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace MVVMCrud.Views.Base
{
    public class BaseContentPage : ContentPage
    {
        public BaseContentPage()
        {
            On<iOS>().SetUseSafeArea(true);

            SetBinding(TitleProperty, new Binding() { Path = "TitlePage" });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Send(this as ContentPage, "Page_OnAppearing");
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            MessagingCenter.Send(this as ContentPage, "Page_OnDisappearing");
        }
    }
}
