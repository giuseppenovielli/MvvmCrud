using System.Windows.Input;
using MVVMCrud.Services.Request;
using Prism.Navigation;

namespace MVVMCrud.ViewModels.Base
{
    public class BaseItemViewModel : BasePageViewModel
    {
        public EmptyViewModel EmptyVM { get; }

        public bool ItemIsVisible { get; set; }

        public bool IsRefreshing { get; set; }
        public ICommand RefreshCommand { get; private set; }

        public BaseItemViewModel(INavigationService navigationService) : base(navigationService)
        {

            EmptyVM = new EmptyViewModel();
            EmptyVM.OnRefresh += delegate
            {
                PageRefresh();
            };
            RefreshCommand = new Command(PageRefresh);

        }

        public virtual void PageRefresh()
        {
            
        }

        public virtual void HideMessage()
        {
            EmptyVM.HideMessage();
            ItemIsVisible = true;

        }

        public virtual void ShowMessage(string message, bool showRefresh = false)
        {
            EmptyVM.ShowMessage(message);
            EmptyVM.BtnRefreshIsVisible = showRefresh;
            ItemIsVisible = false;
        }


    }
}
