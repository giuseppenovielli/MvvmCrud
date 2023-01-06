using System;
using System.Windows.Input;
using MVVMCrud.Services.Request;
using MVVMCrud.Views;
using Prism.Navigation;
using Xamarin.Forms;

namespace MVVMCrud.ViewModels.Base
{
    public class BaseListViewModel : BasePageViewModel
    {
        public EmptyViewModel EmptyVM { get; }
        public EmptyView EmptyView { get; set; }


        public bool IsRefreshing { get; set; }
        public ICommand RefreshCommand { get; private set; }

        public bool ListViewIsVisible { get; set; }

        object _listSelectedItem;
        public object ListSelectedItem
        {
            get { return _listSelectedItem; }
            set
            {
                SetProperty(ref _listSelectedItem, value);
                OnListSelectedItem(value);
            }
        }


        public int ArrayLenght;

        public string SearchBarPlaceholder { get; set; }
        public string SearchBarMessageNotFound { get; set; }
        public bool SearchBarIsVisible { get; set; }
        public double SearchBarOpacity { get; set; }
        string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                SetProperty(ref _searchText, value);
                PerformSearch(value);
            }
        }

        public BaseListViewModel(
            INavigationService navigationService,
            IRequestService requestService) : base(navigationService, requestService)
        {

            EmptyVM = new EmptyViewModel();

            EmptyView = new EmptyView()
            {
                BindingContext = EmptyVM
            };

            EmptyVM.OnRefresh += delegate
            {
                ListViewRefresh();
            };

            RefreshCommand = new Command(ListViewRefresh);

            TlbAddCommandClick = new Command(TlbAddClick);
            TlbSendCommandClick = new Command(TlbSendClick);
            TlbOptionsCommandClick = new Command(TlbOptionsClick);

            SetSearchBarPlaceholder(GetSearchPlaceholder());
            SetSearchBarMessageNotFound(GetNotFound());

            SearchBarOpacity = 1;
            SearchBarIsVisible = true;
        }

        public virtual string GetSearchPlaceholder()
        {
            return MVVMCrudApplication.GetSearchPlaceholder();
        }
        public virtual string GetNotFound()
        {
            return MVVMCrudApplication.GetNotFound();
        }

        public virtual void TlbOptionsClick(object obj)
        {
           
        }

        public virtual void TlbSendClick()
        {
        }

        public virtual void TlbAddClick()
        {

        }

        public virtual void SetSearchBarPlaceholder(string placeholderText)
        {
            if (!string.IsNullOrWhiteSpace(placeholderText))
            {
                SearchBarPlaceholder = placeholderText;
            }
        }

        public virtual void SetSearchBarMessageNotFound(string messageNotFound)
        {
            if (!string.IsNullOrWhiteSpace(messageNotFound))
            {
                SearchBarMessageNotFound = messageNotFound;
            }
        }

        public virtual void ListViewRefresh()
        {
            
        }

        public virtual void HideMessage(bool addSearchBar = true, bool listViewIsVisible = true)
        {

            EmptyVM.HideMessage();
            ListViewIsVisible = listViewIsVisible;

            if (addSearchBar)
            {
                SearchBarOpacity = 1;
                SearchBarIsVisible = true;
            }

        }

        public virtual void ShowMessage(string message, bool showRefresh = false, bool removeSeachBar = false, bool listViewIsVisible = false)
        {
            EmptyVM.ShowMessage(message);
            EmptyVM.BtnRefreshIsVisible = showRefresh;
            ListViewIsVisible = listViewIsVisible;

            if (removeSeachBar)
            {
                SearchBarOpacity = 0;
                SearchBarIsVisible = false;
                
            }

        }

        public virtual void PerformSearch(string newText)
        {
        }

        public virtual void EndLoadingMore()
        {
            IsRefreshing = false;
        }

        public virtual void OnListSelectedItem(object item)
        {
            
        }

    }
}
