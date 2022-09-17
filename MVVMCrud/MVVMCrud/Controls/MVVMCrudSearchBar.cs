using Xamarin.Forms;

namespace MVVMCrud.Controls
{
    public class MVVMCrudSearchBar : SearchBar
    {
        public MVVMCrudSearchBar()
        {
            SetBinding(TextProperty, new Binding() { Path = "SearchText" });
            SetBinding(PlaceholderProperty, new Binding() { Path = "SearchBarPlaceholder" });
            SetBinding(IsVisibleProperty, new Binding() { Path = "SearchBarIsVisible" });
            HorizontalOptions = LayoutOptions.FillAndExpand;

            Focused += (object sender, FocusEventArgs e) =>
            {
                MessagingCenter.Send(this as object, "SearchBar_Focused", e.IsFocused);
            };
        }
    }
}
