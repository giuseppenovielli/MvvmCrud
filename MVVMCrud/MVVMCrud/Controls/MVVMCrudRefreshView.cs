using System.Windows.Input;
using Xamarin.Forms;

namespace MVVMCrud.Controls
{
    public class MVVMCrudRefreshView : RefreshView
    {
        public ICommand RefreshCommand { get; set; }

        public MVVMCrudRefreshView()
        {
            SetBinding(IsRefreshingProperty, new Binding() { Path = "IsRefreshing", Mode=BindingMode.TwoWay});
            SetBinding(CommandProperty, new Binding() { Path = nameof(RefreshCommand) });
        }
    }
}

