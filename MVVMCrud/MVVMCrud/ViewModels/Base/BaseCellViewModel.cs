using System.Windows.Input;
using Prism.Navigation;

namespace MVVMCrud.ViewModels.Base
{
    public class BaseCellViewModel<TItem> : BaseViewModel
    {
        public TItem Item { get; set; }

        public ICommand DeleteClickCommand { get; set; }
        public ICommand EditClickCommand { get; set; }
        public ICommand DetailPageClickCommand { get; set; }

        public BaseCellViewModel(
            TItem item,
            INavigationService navigationService = null) : base(navigationService)
        {
            Item = item;
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
