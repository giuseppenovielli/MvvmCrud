using System.Windows.Input;
using Xamarin.Forms;

namespace MVVMCrud.Controls
{
    public class MVVMCrudListView : ListView
    {
        public ICommand RefreshCommand { get; set; }

        public MVVMCrudListView(ListViewCachingStrategy strategy) : base(strategy)
        {
            SetBinding(SelectedItemProperty, new Binding() { Path = "ListSelectedItem" });
            SetBinding(IsRefreshingProperty, new Binding() { Path = "IsRefreshing" });
            SetBinding(RefreshCommandProperty, new Binding() { Path = nameof(RefreshCommand) });
            SetBinding(FooterProperty, new Binding() { Path = "LoadingMoreView" });

            HasUnevenRows = true;

            ItemAppearing += (sender, e) =>
            {
                MessagingCenter.Send(this as object, "ListView_ItemAppearing", e.Item);
            };

            MessagingCenter.Subscribe<object, object>(this, "ListView_OnScrool", (sender, args) =>
            {
                ScrollTo(args, ScrollToPosition.MakeVisible, true);
            });
        }
    }
}
