using System.Threading.Tasks;
using System.Windows.Input;
using MVVMCrud.ViewModels.Base;
using Xamarin.Forms;

namespace MVVMCrud.Controls
{
    public class MVVMCrudListView : ListView
    {
        public ICommand RefreshCommand { get; set; }

        public bool EventsIsRegistered { get; set; }

        public MVVMCrudListView(ListViewCachingStrategy strategy) : base(strategy)
        {
            var uuid = MVVMCrudApplication.GetLastPageUUID();


            SetBinding(SelectedItemProperty, new Binding() { Path = "ListSelectedItem" });
            SetBinding(IsRefreshingProperty, new Binding() { Path = "IsRefreshing" });
            SetBinding(RefreshCommandProperty, new Binding() { Path = nameof(RefreshCommand) });
            SetBinding(FooterProperty, new Binding() { Path = "LoadingMoreView" });
            SetBinding(ItemsSourceProperty, new Binding() { Path = "ItemsSource" });

            HasUnevenRows = true;
            SeparatorVisibility = SeparatorVisibility.None;
            IsPullToRefreshEnabled = true;
            SelectionMode = ListViewSelectionMode.None;

            ItemAppearing += (sender, e) =>
            {
                var message = string.Format("ListView_ItemAppearing {0}", uuid);
                MessagingCenter.Send(this as object, message, e.Item);

            };


            var message2 = string.Format("ListView_OnScrool {0}", uuid);
            MessagingCenter.Subscribe<object, ScroolToItem>(this, message2, async (sender, args) =>
            {
                if (args != null)
                {
                    ScrollTo(args.Item, ScrollToPosition.Center, true);
                    if (args.IsAnimate)
                    {
                        await Task.Delay(600);
                        var message3 = string.Format("ListView_ScroolToItemAnimate {0}", uuid);
                        MessagingCenter.Send(this as object, message3, args);

                    }

                }
            });
        }
    }
}
