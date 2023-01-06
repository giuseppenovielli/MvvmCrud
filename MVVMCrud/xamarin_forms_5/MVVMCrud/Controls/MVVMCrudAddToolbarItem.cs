using System.Windows.Input;
using Xamarin.Forms;

namespace MVVMCrud.Controls
{
    public class MVVMCrudAddToolbarItem : ToolbarItem
    {
        public ICommand TlbAddCommandClick { get; set; }

        public MVVMCrudAddToolbarItem()
        {
            Text = AppResources.action_add;
            SetBinding(CommandProperty, new Binding() { Path = nameof(TlbAddCommandClick) });
        }
    }
}
