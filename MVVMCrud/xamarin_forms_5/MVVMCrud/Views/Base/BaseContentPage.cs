using System;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace MVVMCrud.Views.Base
{
    public class BaseContentPage : ContentPage
    {
        private readonly string _uuid;

        public BaseContentPage()
        {
            _uuid = Guid.NewGuid().ToString();
            MVVMCrudApplication.Instance?.IdMessagingCenterActiveList.Add(_uuid);

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

            MVVMCrudApplication.Instance?.IdMessagingCenterActiveList.Remove(_uuid);

            MessagingCenter.Send(this as ContentPage, "Page_OnDisappearing");

            System.Diagnostics.Debug.WriteLine("disappearing");

            
        }
    }
}
