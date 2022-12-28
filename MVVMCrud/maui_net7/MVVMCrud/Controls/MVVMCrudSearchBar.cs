

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
            HorizontalOptions = LayoutOptions.FillAndExpand;

            Focused += (object sender, FocusEventArgs e) =>
            {
              var message = string.Format("SearchBar_Focused {0}", uuid);
              MessagingCenter.Send(this as object, message, e.IsFocused);
            };
        }
    }
}
