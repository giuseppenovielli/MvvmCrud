using System.Windows.Input;
using Xamarin.Forms;

namespace MVVMCrud.Controls
{
    public class MVVMCrudCollectionView : CollectionView
    {
        public ICommand ItemsThresholdReachedCommand { get; set; }

        public MVVMCrudCollectionView()
        {
            SetBinding(SelectedItemProperty, new Binding() { Path = "ListSelectedItem" });
            SetBinding(RemainingItemsThresholdProperty, new Binding() { Path = "ItemTreshold" });
            SetBinding(RemainingItemsThresholdReachedCommandProperty, new Binding() { Path = nameof(ItemsThresholdReachedCommand) });
            SetBinding(EmptyViewProperty, new Binding() { Path = "EmptyView" });
            SetBinding(FooterProperty, new Binding() { Path = "LoadingMoreView" });

            MessagingCenter.Subscribe<object, object>(this, "ListView_OnScrool", (sender, args) =>
            {
                ScrollTo(args);
            });
        }

    }
}
