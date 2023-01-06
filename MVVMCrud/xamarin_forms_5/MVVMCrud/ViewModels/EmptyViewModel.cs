using System;
using System.Windows.Input;
using MVVMCrud.ViewModels.Base;
using Prism.Navigation;
using Xamarin.Forms;

namespace MVVMCrud.ViewModels
{
    public class EmptyViewModel : BaseViewModel
    {

        public string RefreshText { get; set; }
        public string Message { get; set; }
        public bool EmptyPageIsVisible { get; set; }
        public bool BtnRefreshIsVisible { get; set; }

        public event EventHandler OnRefresh;

        public ICommand RefreshClickCommand { get; private set; }

        

        public EmptyViewModel(
            INavigationService navigationService) : base(navigationService)
        {
            RefreshText = GetRefreshText();
            RefreshClickCommand = new Command(RefreshClick);

            BtnRefreshIsVisible = false;
        }

        public virtual string GetRefreshText()
        {
            return MVVMCrudApplication.GetRefreshText();
        }

        private void RefreshClick(object obj)
        {
            OnRefresh?.Invoke(this, EventArgs.Empty);
        }

        public void ShowMessage(string message, bool showReload = false)
        {
            Message = message;

            EmptyPageIsVisible = true;

            BtnRefreshIsVisible = showReload;

        }

        public void HideMessage()
        {
            EmptyPageIsVisible = false;
        }


    }
}
