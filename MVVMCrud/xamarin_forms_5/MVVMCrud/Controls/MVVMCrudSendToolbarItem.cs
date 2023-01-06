using System.Windows.Input;


namespace MVVMCrud.Controls
{
    public class MVVMCrudSendToolbarItem : ToolbarItem
    {
        public ICommand TlbSendCommandClick { get; set; }

        public MVVMCrudSendToolbarItem()
        {
            Text = AppResources.action_send;
            SetBinding(CommandProperty, new Binding() { Path = nameof(TlbSendCommandClick) });

        }
    }
}
