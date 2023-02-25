

using CommunityToolkit.Mvvm.Messaging;

namespace MVVMCrud.Controls
{
    public class MVVMCrudSearchBar : SearchBar
    {
        public MVVMCrudSearchBar()
        {
          var uuid = MVVMCrudApplication.GetLastPageUUID();
          
            SetBinding(TextProperty, new Binding() { Path = "SearchText" });
            SetBinding(PlaceholderProperty, new Binding() { Path = "SearchBarPlaceholder" });
            SetBinding(IsVisibleProperty, new Binding() { Path = "SearchBarIsVisible" });

            Focused += (object sender, FocusEventArgs e) =>
            {
                //var message = string.Format("SearchBar_Focused {0}", uuid);
                //MessagingCenter.Send(this as object, message, e.IsFocused);

                // Send a message from some other module
                WeakReferenceMessenger.Default.Send(new SearchBarFocusedMessage(uuid, e.IsFocused));
            };
        }
    }

    public class SearchBarFocusedMessage : GenericUUIDChangeMessage<bool>
    {
        public SearchBarFocusedMessage(string uuid, bool isFocused) : base(uuid, isFocused)
        {
        }
    }
}
