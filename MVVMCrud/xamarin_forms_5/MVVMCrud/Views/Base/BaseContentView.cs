using MVVMCrud;
using MVVMCrud.ViewModels.Base;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace MVVMCrud.Views.Base
{
    public class BaseContentView : ContentView
    {
        public BaseContentView()
        {
            this.SetDynamicResource(ContentView.BackgroundColorProperty, "cellBackgroundColor");

            var uuid = MVVMCrudApplication.GetLastPageUUID();
            var message = string.Format("ListView_ScroolToItemAnimate {0}", uuid);
            MessagingCenter.Subscribe<object, ScroolToItem>(this, message, (sender, args) =>
            {
                try
                {
                    if (
                        args != null
                        &&
                        JsonConvert.SerializeObject(BindingContext).Equals(JsonConvert.SerializeObject(args.Item))
                    )
                    {
                        var message2 = string.Format("ListView_ScroolToItem_Animate_To_ViewModel {0}", uuid);
                        MessagingCenter.Send(this as ContentView, message2);
                    }
                }
                catch (System.Exception) { }

            });
        }
    }
}
