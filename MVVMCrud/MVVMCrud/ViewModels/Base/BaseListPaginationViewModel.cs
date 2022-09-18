using System.Threading.Tasks;
using MVVMCrud.Models.ItemRoot;
using MVVMCrud.Services.Request;
using MVVMCrud.Views;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace MVVMCrud.ViewModels.Base
{
    public class BaseListPaginationViewModel : BaseListViewModel
    {
        public LoadingMoreViewModel LoadingMoreVM { get; set; }
        public LoadingMoreView LoadingMoreView { get; private set; }

        public DelegateCommand ItemsThresholdReachedCommand { get; private set; }
        public int ItemTreshold { get; set; }
        public bool IsLoadingMore { get; set; }
        public PaginationItem PaginationItem;
        public bool ViewMore;


        public BaseListPaginationViewModel(
            INavigationService navigationService,
            IRequestService requestService = null) : base(navigationService, requestService)
        {
            ItemsThresholdReachedCommand = new DelegateCommand(ItemsThresholdReached, CanExecuteLoadingMore).ObservesProperty(() => ItemTreshold);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            LoadingMoreVM = new LoadingMoreViewModel(NavigationService)
            {
                LoadingMoreClickCommand = new Command((obj) =>
                {
                    ItemsThresholdReached();
                })
            };
            LoadingMoreView = new LoadingMoreView()
            {
                BindingContext = LoadingMoreVM
            };
        }

        public virtual bool SetupIsPaginationEnable()
        {
            return true;
        }

        public virtual ContentView GetLoadingMoreView()
        {
            return MVVMCrudApplication.GetLoadingMoreView();
        }

        bool CanExecuteLoadingMore()
        {
            return ItemTreshold == 1 ? true : false;
        }


        public virtual void ItemsThresholdReached()
        {
            if (ViewMore && !IsLoadingMore && PaginationItem != null && !string.IsNullOrWhiteSpace(PaginationItem.NextUrl))
            {
                System.Diagnostics.Debug.WriteLine("Loading More IN");
                LoadingMore();
            }
        }

        public virtual void LoadingMore()
        {
            IsLoadingMore = true;

            LoadingMoreVM.ShowLoadingMore();
        }

        public virtual void ProcessPagination()
        {
            System.Diagnostics.Debug.WriteLine("ArrayLenght = " + ArrayLenght.ToString());

            if (PaginationItem != null && !string.IsNullOrWhiteSpace(PaginationItem.NextUrl))
            {
                ItemTreshold = 1;

                ViewMore = true;
            }
            else
            {
                ViewMore = false;

                ItemTreshold = -1;

            }

        }

        public override void ShowMessage(string message, bool showRefresh = false, bool removeSeachBar = false)
        {
            base.ShowMessage(message, showRefresh, removeSeachBar);

            LoadingMoreVM.HideLoadingMore();
        }

        public override void HideMessage(bool addSearchBar = true)
        {
            base.HideMessage(addSearchBar);

            EndLoadingMore();
        }

        public override async void EndLoadingMore()
        {
            base.EndLoadingMore();

            IsLoadingMore = false;
            LoadingMoreVM.HideLoadingMore();

            await Task.Delay(400);
            if (!string.IsNullOrWhiteSpace(PaginationItem?.NextUrl)
                &&
                !IsLoadingMore)
            {
                LoadingMoreVM.ShowLoadingMore(false, true);
            }
        }

        public override void ListViewRefresh()
        {
            base.ListViewRefresh();

            LoadingMoreVM.HideLoadingMore();
        }
    }
}
