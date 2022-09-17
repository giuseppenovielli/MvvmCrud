using System.Windows.Input;
using MVVMCrud.ViewModels.Base;
using Prism.Navigation;

namespace MVVMCrud.ViewModels
{

    public class LoadingMoreViewModel : BaseViewModel
    {
        public string LoadingMoreText { get; }

        public bool BtnLoadingMoreIsVisible { get; set; }
        public bool ActivityIndicatorIsVisible { get; set; }
        public bool LoadingMoreIsVisible { get; set; }

        public ICommand LoadingMoreClickCommand { get; set; }

        public LoadingMoreViewModel(
            INavigationService navigationService) : base(navigationService)
        {
            LoadingMoreIsVisible = false;
            BtnLoadingMoreIsVisible = false;
            ActivityIndicatorIsVisible = false;

            LoadingMoreText = GetLoadingMoreText();

        }

        public virtual string GetLoadingMoreText()
        {
            return MVVMCrudApplication.GetLoadingMoreText();
        }

        public void ShowLoadingMore(bool showActivityIndicator = true, bool showButton = false)
        {
            BtnLoadingMoreIsVisible = showButton;
            ActivityIndicatorIsVisible = showActivityIndicator;
            LoadingMoreIsVisible = true;
        }

        public void HideLoadingMore(bool showActivityIndicator = false, bool showButton = true)
        {
            BtnLoadingMoreIsVisible = showButton;
            ActivityIndicatorIsVisible = showActivityIndicator;
            LoadingMoreIsVisible = false;
        }
    }
}
