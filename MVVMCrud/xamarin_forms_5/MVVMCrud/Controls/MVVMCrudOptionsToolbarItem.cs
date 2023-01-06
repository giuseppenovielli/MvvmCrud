﻿using System.Windows.Input;


namespace MVVMCrud.Controls
{
    public class MVVMCrudOptionsToolbarItem : ToolbarItem
    {
        public ICommand TlbOptionsCommandClick { get; set; }

        public MVVMCrudOptionsToolbarItem()
        {
            Text = AppResources.action_options;
            SetBinding(CommandProperty, new Binding() { Path = nameof(TlbOptionsCommandClick) });
        }
    }
}
