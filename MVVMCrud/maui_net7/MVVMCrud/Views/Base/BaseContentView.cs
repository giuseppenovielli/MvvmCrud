using MVVMCrud.ViewModels.Base;
using Newtonsoft.Json;

namespace MVVMCrud.Views.Base
{
    public class BaseContentView : ContentView
    {
        public BaseContentView()
        {
            var uuid = MVVMCrudApplication.GetLastPageUUID();

            var message = string.Format("ListView_ScroolToItemAnimate {0}", uuid);
            MessagingCenter.Subscribe<object, ScroolToItem>(this, message, (sender, args) =>
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
            });

        }
    }
}
