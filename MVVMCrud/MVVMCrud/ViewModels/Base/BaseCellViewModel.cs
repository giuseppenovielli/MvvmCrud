using System.Windows.Input;
using MVVMCrud.Services.Request;
using Prism.Navigation;

namespace MVVMCrud.ViewModels.Base
{
    public class BaseCellViewModel<TItem> : BaseViewModel
    {
        public TItem Item { get; set; }

        public bool IsSelected { get; set; }

        public ICommand DeleteClickCommand { get; set; }
        public ICommand EditClickCommand { get; set; }
        public ICommand DetailPageClickCommand { get; internal set; }
	public ICommand SelectClickCommand { get; set; }

        public BaseCellViewModel(
            TItem item,
            INavigationService navigationService = null,
            IRequestService requestService = null) : base(navigationService, requestService)
        {
            Item = item;
        }

        public BaseCellViewModel(
            INavigationService navigationService = null,
            IRequestService requestService = null) : base(navigationService, requestService)
        {
        }

        public BaseCellViewModel() { }

        public virtual void Initialize(TItem item)
        {
            Item = item;
            SetupInterface();
        }

        public virtual void SetupInterface()
        {
            
        }
    }
}
