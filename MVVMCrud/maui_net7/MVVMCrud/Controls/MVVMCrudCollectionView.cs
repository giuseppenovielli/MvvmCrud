using System.Threading.Tasks;
using System.Windows.Input;
using MVVMCrud.ViewModels.Base;


namespace MVVMCrud.Controls
{
    public class MVVMCrudCollectionView : CollectionView
    {
        public ICommand ItemsThresholdReachedCommand { get; set; }

        public MVVMCrudCollectionView()
        {
            var uuid = MVVMCrudApplication.GetLastPageUUID();

            SetBinding(SelectedItemProperty, new Binding() { Path = "ListSelectedItem" });
            SetBinding(RemainingItemsThresholdProperty, new Binding() { Path = "ItemTreshold" });
            SetBinding(RemainingItemsThresholdReachedCommandProperty, new Binding() { Path = nameof(ItemsThresholdReachedCommand) });
            SetBinding(EmptyViewProperty, new Binding() { Path = "EmptyView" });
            SetBinding(FooterProperty, new Binding() { Path = "LoadingMoreView" });
            SetBinding(ItemsSourceProperty, new Binding() { Path = "ItemsSource" });

            var message = string.Format("ListView_OnScrool {0}", uuid);
            MessagingCenter.Subscribe<object, ScroolToItem>(this, message, async (sender, args) =>
            {
                if (args != null)
                {
                    ScrollTo(args.Item, -1, ScrollToPosition.Center);
                    await Task.Delay(300);

                    if (args.IsAnimate)
                    {
                        var message2 = string.Format("ListView_ScroolToItemAnimate {0}", uuid);
                        MessagingCenter.Send(this as object, message2, args);
                    }

                }
                
            });
        }

    }
}
