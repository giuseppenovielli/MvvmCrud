using System.Windows.Input;
using MVVMCrud.Services.Request;
using Prism.Navigation;
using Xamarin.Forms;

namespace MVVMCrud.ViewModels.Base
{
    public class BaseItemEditViewModel : BaseViewModel
    {
        public EmptyViewModel EmptyVM { get; set; }

        public bool ItemIsVisible { get; set; }

        public ICommand TlbAddCommandClick { get; private set; }
        public ICommand TlbSendCommandClick { get; private set; }

        public BaseItemEditViewModel(
            INavigationService navigationService,
            IRequestService requestService = null) : base(navigationService, requestService)
        {
            TlbAddCommandClick = new Command(TlbAddClick);
            TlbSendCommandClick = new Command(TlbSendClick);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            EmptyVM = new EmptyViewModel(NavigationService);
            EmptyVM.OnRefresh += delegate
            {
                PageRefresh();
            };
        }

        public virtual void PageRefresh()
        {
        }

        public virtual void HideMessage()
        {
            EmptyVM.HideMessage();
            ItemIsVisible = true;

        }

        public virtual void TlbSendClick()
        {
        }

        public virtual void TlbAddClick()
        {

        }

        public virtual void ShowMessage(string message, bool showRefresh = false)
        {
            EmptyVM.ShowMessage(message);
            EmptyVM.BtnRefreshIsVisible = showRefresh;
            ItemIsVisible = false;
        }
    }
}
