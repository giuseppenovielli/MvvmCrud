using System.Net.Http;
using System.Windows.Input;
using MVVMCrud.Services.Request;
using Prism.Navigation;

namespace MVVMCrud.ViewModels.Base
{
    public class BasePageViewModel : BaseViewModel, IDestructible, IApplicationLifecycleAware, IPageLifecycleAware, INavigationAware, IInitialize, INavigationPageOptions
    {
        public string Endpoint { get; set; }

        public string TitlePage { get; set; }
        public bool Start;

        public IRequestService RequestService { get; }
        public INavigationService NavigationService { get; }

        public ICommand TlbAddCommandClick { get; set; }
        public ICommand TlbSendCommandClick { get; set; }
        public ICommand TlbOptionsCommandClick { get; set; }
        public ICommand TlbInfoCommandClick { get; set; }

        public bool ClearNavigationStackOnNavigation => throw new NotImplementedException();

        public BasePageViewModel(
            INavigationService navigationService,
            IRequestService requestService = null)
        {
            NavigationService = navigationService;
            RequestService = requestService;
        }

        public virtual void Initialize(INavigationParameters parameters)
        {
            Start = true;
            InitializeParameters(parameters);
        }


        public virtual void InitializeParameters(INavigationParameters parameters)
        {
        }

        public virtual string SetupEndpoint() => string.Empty;

        public virtual string SetupTitlePage() => string.Empty;

        public virtual HttpClient GetHttpClient()
        {
            return MVVMCrudApplication.Instance.HttpClient;
        }

        public async Task PopModalAsync()
        {
            //var navResult = await NavigationService?.GoBackAsync(useModalNavigation: true);
            var navResult = await NavigationService?.GoBackAsync();
        }

        public virtual void OnResume()
        {

        }

        public virtual void OnSleep()
        {

        }

        public virtual void OnAppearing()
        {

        }

        public virtual void OnDisappearing()
        {

        }


        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }


        public virtual void Destroy()
        {

        }
    }
}
