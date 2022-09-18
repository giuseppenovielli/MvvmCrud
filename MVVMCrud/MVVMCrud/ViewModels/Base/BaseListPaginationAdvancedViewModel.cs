using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MVVMCrud.CustomView;
using MVVMCrud.Models.Base;
using MVVMCrud.Models.ItemRoot;
using MVVMCrud.Services.Request;
using MVVMCrud.Utils;
using Newtonsoft.Json;
using Prism.Navigation;
using Xamarin.Forms;

namespace MVVMCrud.ViewModels.Base
{
    public class BaseListPaginationAdvancedViewModel<TCellVM, TItemsRoot, TItem> : BaseListPaginationViewModel
        where TCellVM : BaseCellViewModel<TItem>, new()
        where TItem : BaseItem, new()
        where TItemsRoot : BaseModelItemsRoot<TItem>, new()
    {
        public ObservableCollection<TCellVM> ItemsList { get; set; }
        public ObservableCollection<TCellVM> ItemsListSearch { get; set; }
        public ObservableCollection<TCellVM> ItemsSource { get; set; }

        public event EventHandler<int> OnScroolToIndex;

        public BaseListPaginationAdvancedViewModel(
            INavigationService navigationService,
            IRequestService requestService) : base(navigationService, requestService)
        {
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            TitlePage = SetupTitlePage();
            Endpoint = SetupEndpoint();

            _ = SetupGet();
        }

        public override string SetupTitlePage()
        {
            var pageVMName = GetType().Name;

            var pageContext = pageVMName.Split(new[] { "PageViewModel" }, StringSplitOptions.None);
            if (pageContext?.Length == 2)
            {
                var pageName = pageContext[0].ToLower();

                var label = string.Format("title_page_{0}", pageName);
                return MVVMCrudApplication.GetAppResourceManager().GetString(label);
            }

            return null;
        }

        public virtual FormUrlEncodedContent SetupContentQuery()
        {
            return null;
        }

        public async virtual Task SetupGet()
        {
            await GetItems(pagination: SetupIsPaginationEnable());
        }

        public virtual async Task<TItemsRoot> SetupGetItemsRequest(string urlPagination = null, bool pagination = true)
        {
            if (!string.IsNullOrWhiteSpace(Endpoint))
            {
                return await RequestService.List<TItemsRoot, TItem>(Endpoint, SetupContentQuery(), urlPagination, pagination);
            }

            return await Task.FromResult<TItemsRoot>(null);
        }

        public virtual TCellVM SetupInstanceCell(TItem item)
        {
            var cell = new TCellVM();
            cell.Initialize(item);
            return cell;
        }

        public virtual void SetupGetItemsExtra(TItemsRoot obj)
        {

        }

        public virtual List<TCellVM> PerformSearchSetup(string newText)
        {
            return null;
        }


        public override void ListViewRefresh()
        {
            base.ListViewRefresh();

            _ = SetupGet();
        }

        public async Task GetItems(string urlPagination = null, bool pagination = true)
        {
            var rootItemBase = await RequestService.RequestItems<TItemsRoot, TItem>
                (
                    async (string arg1, bool arg2) =>
                    {
                        return await SetupGetItemsRequest(urlPagination, pagination);
                    },
                TitlePage,
                    (string obj) =>
                    {
                        SetupGetItemsPagination(urlPagination);
                    },
                urlPagination, pagination);


            if (rootItemBase != null)
            {
                PaginationItem = rootItemBase.PaginationItem;
                GetItemsSetupList(rootItemBase);
            }

            SetupLoadingComplete();
        }


        public virtual void SetupLoadingComplete()
        {
            ProcessPagination();

            SetupLoadingCompleteItems();

            EndLoadingMore();
        }

        public override void LoadingMore()
        {
            base.LoadingMore();

            _ = GetItems(PaginationItem.NextUrl);
        }

        public virtual async Task<bool> SearchBarFocused(bool isFocused)
        {
            if (isFocused)
            {
                if (!string.IsNullOrWhiteSpace(PaginationItem?.NextUrl))
                {
                    await GetItems(pagination: false);
                    return true;
                }
            }

            return false;

        }

        public virtual string GetLoadingText()
        {
            return MVVMCrudApplication.GetLoadingText();
        }

        public virtual void SetupGetItemsPagination(string urlPagination = null)
        {
            if (string.IsNullOrWhiteSpace(urlPagination))
            {
                ShowMessage(GetLoadingText());
                ItemsList = new ObservableCollection<TCellVM>();
                ItemsSource = new ObservableCollection<TCellVM>();
                ViewMore = false;
            }
        }

        void GetItemsSetupList(TItemsRoot obj)
        {
            SetupGetItemsExtra(obj);

            var items = obj.Items;
            ArrayLenght = items.Count;

            if (ArrayLenght > 0)
            {
                foreach (var item in items)
                {
                    var cellVM = InstanceCellVM(item);
                    if (cellVM != null)
                    {
                        ItemsList.Add(cellVM);
                    }
                }
            }
        }


        TCellVM InstanceCellVM(TItem item)
        {
            if (item != null)
            {
                var cellVM = SetupInstanceCell(item);
                if (cellVM != null)
                {
                    cellVM.DeleteClickCommand = new Command<TCellVM>(async (obj) =>
                    {
                        SetupOnDeletedItem(obj, await SetupUploadDeleteItem(obj));
                    });

                    cellVM.EditClickCommand = new Command<TCellVM>((obj) =>
                    {
                        SetupEditItem(obj);

                    });

                    cellVM.DetailPageClickCommand = new Command<TCellVM>((obj) =>
                    {
                        SetupDetailPage(obj);

                    });

                    return cellVM;
                }
            }

            return null;
        }

        public virtual string SetupDetailPageName()
        {
            return string.Empty;
        }

        public virtual async void SetupDetailPage(TCellVM obj)
        {
            var pageName = SetupDetailPageName();

            var navParams = new NavigationParameters
            {
                { "id", obj.Item.Id },
                { "position", ItemsSource.IndexOf(obj) }
            };
            var navResult = await NavigationService.NavigateAsync(pageName, navParams);
        }

        public virtual string GetConfirmDeleteText()
        {
            return MVVMCrudApplication.GetConfirmDeleteText();
        }

        public virtual string GetOKText()
        {
            return MVVMCrudApplication.GetYesText();
        }

        public virtual string GetCancelText()
        {
            return MVVMCrudApplication.GetNoText();
        }

        public virtual async void SetupEditItem(TCellVM cellVM)
        {
            var navParams = new NavigationParameters
            {
                { "endpoint", SetupCreateUpdatePageEndpoint() },
                { "itemSerialized", SetupCreateUpdatePageItemSerialized(cellVM) },
                { "position", ItemsList.IndexOf(cellVM) }
            };

            var page = SetupCreateUpdatePage();
            if (!string.IsNullOrWhiteSpace(page))
            {
                var navResult = await NavigationService.NavigateAsync(page, navParams, useModalNavigation: SetupCreateUpdatePageIsModal());

            }
        }

        public virtual JsonSerializerSettings SetupJsonSerializerSettings()
        {
            return MVVMCrudApplication.Instance.SetupJsonSettingsSerialize();
        }

        public virtual string SetupCreateUpdatePageItemSerialized(TCellVM cellVM)
        {
            var item = cellVM.Item;
            if (item != null)
            {
                var settings = SetupJsonSerializerSettings();
                return JsonConvert.SerializeObject(item, settings);
            }
            return null;
        }

        public override async void TlbAddClick()
        {
            var page = SetupCreateUpdatePage();
            if (!string.IsNullOrWhiteSpace(page))
            {
                var navParams = new NavigationParameters
                {
                    { "endpoint", SetupCreateUpdatePageEndpoint() },
                };

                var navResult = await NavigationService.NavigateAsync(page, navParams, useModalNavigation: SetupCreateUpdatePageIsModal());
            }
        }

        public virtual string SetupCreateUpdatePageName()
        {
            return GetType().Name;
        }

        public virtual string SetupCreateUpdatePage()
        {
            var pageVMName = SetupCreateUpdatePageName();

            var pageContext = pageVMName.Split(new[] { "PageViewModel" }, StringSplitOptions.None);
            if (pageContext?.Length == 2)
            {
                var pageName = pageContext[0];

                var pageNewEdit = pageName + "NewEditPage";

                if (!SetupCreateUpdatePageIsModal())
                {
                    return pageNewEdit;
                }
                else
                {
                    return string.Format("{0}/{1}", nameof(MVVMCrudModalNavigationPage), pageNewEdit);
                }


            }

            return null;
        }

        public virtual bool SetupCreateUpdatePageIsModal()
        {
            return false;
        }

        public virtual string SetupCreateUpdatePageEndpoint()
        {
            return Endpoint;
        }

        public virtual async Task<RootItemBase> SetupUploadDeleteItem(TCellVM cellVM)
        {
            var id = cellVM?.Item?.Id;
            if (!string.IsNullOrWhiteSpace(id)
                &&
                await SetupDeleteMessageConfirm())
            {
                return await RequestService.RequestDefaultDeleteItem(SetupDeleteEndpoint(), id, TitlePage);
            }

            return null;
        }

        public virtual void SetupOnDeletedItem(TCellVM cellVM, RootItemBase rootItemBase)
        {
            if (rootItemBase != null
                &&
                SetupDeletedItem(cellVM))
            {
                SetupDeleteItemMessage();
            }
        }

        public virtual bool SetupDeletedItem(TCellVM cellVM)
        {
            if (cellVM != null)
            {
                var isDeleted = ItemsList.Remove(cellVM);
                if (isDeleted)
                {
                    PerformSearch(SearchText);
                }
                return isDeleted;
            }

            return false;

        }

        public virtual async Task<bool> SetupDeleteMessageConfirm()
        {
            var message = GetConfirmDeleteText();
            var ok = GetOKText();
            var cancel = GetCancelText();

            return await Application.Current.MainPage.DisplayAlert(TitlePage, message, ok, cancel);

        }


        public virtual string SetupDeleteEndpoint()
        {
            return Endpoint;
        }

        public virtual string GetEmptyText()
        {
            return MVVMCrudApplication.GetEmptyText();
        }

        public virtual void SetupLoadingCompleteItems()
        {
            PerformSearch(SearchText);
        }



        public override void PerformSearch(string newText)
        {
            if (ItemsList != null && ItemsList.Count > 0)
            {
                if (!string.IsNullOrWhiteSpace(newText))
                {
                    var itemsResult = PerformSearchSetup(newText);

                    if (itemsResult != null && itemsResult.Count > 0)
                    {
                        ItemsListSearch = new ObservableCollection<TCellVM>(itemsResult);
                        ItemsSource = ItemsListSearch;
                        HideMessage();
                    }
                    else
                    {
                        ShowMessage(SearchBarMessageNotFound);
                        ItemsSource = new ObservableCollection<TCellVM>();
                    }
                }
                else
                {
                    ItemsSource = ItemsList;
                    HideMessage();
                }
            }
            else
            {
                ShowMessage(GetEmptyText(), true, true);
                ItemsSource = ItemsList;
            }
        }

        public virtual void SubscribeListViewItemAppearing()
        {
            MessagingCenter.Subscribe<object, object>(this, "ListView_ItemAppearing", (sender, args) =>
            {
                if (
                    ItemsList != null
                    &&
                    ItemsList.Count > 0
                    &&
                    args == ItemsList[ItemsList.Count - 1]
                )
                {
                    ItemsThresholdReached();
                }

            });
        }

        public virtual void SubscribeSearchBarFocused()
        {
            MessagingCenter.Subscribe<object, bool>(this, "SearchBar_Focused", async (sender, isFocused) =>
            {
                await SearchBarFocused(isFocused);
            });
        }

        public virtual void UnsubscribeListViewItemAppearing()
        {
            MessagingCenter.Unsubscribe<object, object>(this, "ListView_ItemAppearing");
        }

        public virtual void UnsubscribeSearchBarFocused()
        {
            MessagingCenter.Unsubscribe<object, bool>(this, "SearchBar_Focused");
        }

        public virtual void SubscribeMessagingCenter()
        {
            SubscribeListViewItemAppearing();
            SubscribeSearchBarFocused();
        }

        public virtual void UnsubscribeMessagingCenter()
        {
            UnsubscribeListViewItemAppearing();
            UnsubscribeSearchBarFocused();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            SubscribeMessagingCenter();

            if (parameters.ContainsKey("newEditItem"))
            {
                var newEditItem = parameters["newEditItem"] as NewEditItem<TItem>;
                if (newEditItem != null)
                {
                    var pos = newEditItem.Position;
                    if (pos == -1)
                    {
                        //New
                        AddNewItem(newEditItem.Item);
                    }
                    else
                    {
                        //Edit
                        UpdateEditItem(newEditItem);
                    }
                }
            }
            else
            {
                if (parameters.ContainsKey("deleteItem"))
                {
                    var pos = parameters.GetValue<int>("deleteItem");
                    SetupDeleteItem(pos);
                }
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            UnsubscribeMessagingCenter();
        }

        public virtual int AddNewItemPosition() => 0;
        public virtual bool AddNewItemIsScroolEnabled() => true;

        public virtual void SetupDeleteItem(int pos)
        {
            //Delete Item
            if (pos < ItemsList.Count)
            {
                ItemsList.RemoveAt(pos);
                PerformSearch(SearchText);
            }

            SetupDeleteItemMessage();
        }

        public virtual void AddNewItem(TItem item)
        {
            if (item != null)
            {
                var cellVM = SetupInstanceCell(item);
                if (cellVM != null)
                {
                    SetupAddItemMessage();

                    var newItemPos = AddNewItemPosition();

                    ItemsList.Insert(newItemPos, cellVM);

                    PerformSearch(SearchText);

                    if (AddNewItemIsScroolEnabled())
                    {
                        AddNewEditItemScrool(newItemPos);
                    }
                }

            }
        }


        public virtual void AddNewEditItemScrool(int newItemPos)
        {

            var cellVM = ItemsList?.ElementAtOrDefault(newItemPos);
            if (cellVM != null)
            {
                OnScroolToIndex?.Invoke(this, newItemPos);

                var cellVMObj = cellVM as object;
                MessagingCenter.Send(this as object, "ListView_OnScrool", cellVMObj);
            }

        }

        public virtual void UpdateEditItem(NewEditItem<TItem> newEditItem)
        {
            var pos = newEditItem.Position;
            var section = newEditItem.Section;

            if (pos < ItemsList.Count)
            {
                var cellVM = InstanceCellVM(newEditItem.Item);
                if (cellVM != null)
                {
                    ItemsList[pos] = cellVM;
                    SetupEditItemMessage();
                    PerformSearch(SearchText);
                }
            }


        }

        public virtual bool SetupDefaultEditItemMessage()
        {
            return true;
        }

        public virtual void SetupEditItemMessage()
        {
            if (SetupDefaultEditItemMessage())
            {
                Utils.Utils.DisplaySimplyAlert(TitlePage, MVVMCrudApplication.GetEditItemUploadText());
            }
        }

        public virtual bool SetupDefaultAddItemMessage()
        {
            return true;
        }

        public virtual void SetupAddItemMessage()
        {
            if (SetupDefaultAddItemMessage())
            {
                Utils.Utils.DisplaySimplyAlert(TitlePage, MVVMCrudApplication.GetNewItemUploadText());
            }
        }


        public virtual bool SetupDefaultDeleteItemMessage()
        {
            return true;
        }
        public virtual void SetupDeleteItemMessage()
        {
            if (SetupDefaultDeleteItemMessage())
            {
                Utils.Utils.DisplaySimplyAlert(TitlePage, MVVMCrudApplication.GetDeleteItemUploadText());
            }
        }

    }
}
