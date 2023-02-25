using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;
using DryIoc;
using MVVMCrud.ViewModels.Base;

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
                //var message = string.Format("ListView_ItemAppearing {0}", uuid);
                //MessagingCenter.Send(this as object, message, e.Item);

                // Send a message from some other module
                WeakReferenceMessenger.Default.Send(new ListViewItemAppearingChangeMessage(uuid, e.Item));
            };


            //var message2 = string.Format("ListView_OnScrool {0}", uuid);
            //MessagingCenter.Subscribe<object, ScroolToItem>(this, message2, async (sender, args) =>
            //{
            //    if (args != null)
            //    {
            //        ScrollTo(args.Item, ScrollToPosition.Center, true);
            //        if (args.IsAnimate)
            //        {
            //            await Task.Delay(600);
            //            var message3 = string.Format("ListView_ScroolToItemAnimate {0}", uuid);
            //            MessagingCenter.Send(this as object, message3, args);

            //        }

            //    }
            //});

            // Register a message in some module
            WeakReferenceMessenger.Default.Register<ListViewScroolToMessage>(this, async (r, m) =>
            {
                if (
                        m?.Value != null
                        &&
                        m?.Uuid == uuid
                    )
                {
                    var scroolTo = m.Value;
                    var item = scroolTo.Item;

                    ScrollTo(item, ScrollToPosition.Center, true);
                    if (scroolTo.IsAnimate)
                    {
                        await Task.Delay(500);

                        //var message3 = string.Format("ListView_ScroolToItemAnimate {0}", uuid);
                        //MessagingCenter.Send(this as object, message3, args);

                        // Send a message from some other module
                        WeakReferenceMessenger.Default.Send(new ListViewScroolToItemAnimateMessage(uuid, item));

                    }

                }

            });
        }
    }

    public class ListViewItemAppearingChangeMessage : GenericUUIDChangeMessage<object>
    {
        public ListViewItemAppearingChangeMessage(string uuid, object value) : base(uuid, value)
        {
        }
    }

    public class ListViewScroolToMessage : GenericUUIDChangeMessage<ScroolToItem>
    {
        public ListViewScroolToMessage(string uuid, ScroolToItem value) : base(uuid, value)
        {
        }
    }

    public class ListViewScroolToItemAnimateMessage : GenericUUIDChangeMessage<object>
    {
        public ListViewScroolToItemAnimateMessage(string uuid, object value) : base(uuid, value)
        {
        }
    }

    public class ListViewScroolToItemAnimateToViewModelMessage : GenericUUIDChangeMessage<ContentView>
    {
        public ListViewScroolToItemAnimateToViewModelMessage(string uuid, ContentView value) : base(uuid, value)
        {
        }
    }
}
