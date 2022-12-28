using MVVMCrud.Services.Request;
using Prism.AppModel;
using Prism.Mvvm;
using Prism.Navigation;
using System.Net.Http;
using System.Threading.Tasks;

namespace MVVMCrud.ViewModels.Base
{
    public abstract class BaseViewModel : BindableBase, IDestructible, IApplicationLifecycleAware, IPageLifecycleAware, INavigationAware, IInitialize, INavigationPageOptions
    {
        public bool Start;
        public string Endpoint { get; set; }
        public virtual string TitlePage { get; set; }

        public IRequestService RequestService { get; }
        public INavigationService NavigationService { get; }

        public BaseViewModel()
        {
        }

        public BaseViewModel(
            INavigationService navigationService = null,
            IRequestService requestService = null)
        {
            NavigationService = navigationService;
            RequestService = requestService;
        }

        public bool ClearNavigationStackOnNavigation => false;

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

        public virtual void Initialize(INavigationParameters parameters)
        {
        }

        public async Task PopModalAsync()
        {
            if (NavigationService != null)
            {
                //var navResult = await NavigationService.GoBackAsync(useModalNavigation: true);
                var navResult = await NavigationService.GoBackAsync();
            }

        }

        public void Destroy()
        {
        }

        public virtual string SetupEndpoint()
        {
            return string.Empty;
        }

        public virtual string SetupTitlePage()
        {
            return string.Empty;
        }

        public virtual HttpClient GetHttpClient()
        {
            var instance = MVVMCrudApplication.Instance;
            if (instance != null)
            {
                return instance.HttpClient;
            }
            return new HttpClient();
        }
    }
}
