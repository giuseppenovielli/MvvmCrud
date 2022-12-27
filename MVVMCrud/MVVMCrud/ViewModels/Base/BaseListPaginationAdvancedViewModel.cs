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
using MVVMCrud.Views.Base;
using Newtonsoft.Json;
using Prism.Navigation;
using Xamarin.Essentials;
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

        readonly string _uuidMessagingCenter;

        public string Id { get; private set; }
        public bool ScrolledToId { get; set; }

        bool _isLoadingData;

        public BaseListPaginationAdvancedViewModel(
            INavigationService navigationService,
            IRequestService requestService) : base(navigationService, requestService)
        {
            _uuidMessagingCenter = MVVMCrudApplication.GetLastPageUUID();
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters.ContainsKey("id")){ Id = parameters.GetValue<string>("id"); }

            TitlePage = SetupTitlePage();
            Endpoint = SetupEndpoint();

            if (string.IsNullOrWhiteSpace(Id))
            {
                _ = SetupGet();
            }
            else
            {
                ShowMessage(MVVMCrudApplication.GetLoadingText());
            }

        }


        public override void OnAppearing()
        {
            base.OnAppearing();

            if (!string.IsNullOrWhiteSpace(Id))
            {
                _ = SetupGet();
            }
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
            await GetItems(pagination: ManageIsPagination());

        }

        public virtual bool ManageIsPagination()
        {
            if (IsScroolToItemInput())
            {
                return true;
            }
            else
            {
                return SetupIsPaginationEnable();
            }
        }

        public virtual bool IsScroolToItemInput()
        {
            return (
                    !string.IsNullOrWhiteSpace(Id)
                    &&
                    !ScrolledToId
                    );
        }


        public virtual async Task<TItemsRoot> SetupGetItemsRequest(string urlPagination = null, bool pagination = true)
        {
            if (!string.IsNullOrWhiteSpace(Endpoint))
            {
                return await RequestService.List<TItemsRoot, TItem>(Endpoint, SetupContentQuery(), urlPagination, pagination, httpClient: GetHttpClient());
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

            _ = SetupGetItems();
        }

        public virtual async Task SetupGetItems()
        {
            await GetItems(pagination: SetupIsPaginationEnable());
        }

        public async Task GetItems(string urlPagination = null, bool pagination = true)
        {
            _isLoadingData = true;

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
                urlPagination, pagination: pagination);


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

            _isLoadingData = false;
        }

        public override void LoadingMore()
        {
            base.LoadingMore();

            _ = GetItems(PaginationItem.NextUrl);
        }

        public bool SearchBarFocused(bool isFocused)
        {
            if (isFocused)
            {
                if (PaginationItem != null
                    &&
                    !string.IsNullOrWhiteSpace(PaginationItem.NextUrl))
                {
                    _ = GetItems(pagination: false);
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
            if (items != null)
            {
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

			        cellVM.SelectClickCommand = new Command<TCellVM>((obj) =>
                    {
                        SetupSelectedItem(obj);
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

        public virtual async void SetupSelectedItem(TCellVM obj)
        {
            var navParams = new NavigationParameters
            {
                { "selectedItem", obj?.Item}
            };

            var navResult = await NavigationService.GoBackAsync(navParams, useModalNavigation: true);

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

        public virtual string SetupCreateUpdatePageItemSerialized(TCellVM cellVM)
        {
            var item = cellVM.Item;
            if (item != null)
            {
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new IgnoreJsonPropertyContractResolver()
                };
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
            if (Device.RuntimePlatform == Device.iOS)
            {
                return true;
            }
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
                return await RequestService.RequestDefaultDeleteItem(SetupDeleteEndpoint(), id, TitlePage, httpClient: GetHttpClient());
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
            ScroolToItemInput();
        }

        public virtual void ScroolToItemInput()
        {
            if (IsScroolToItemInput())
            {
                var item = ItemsSource.Where(x => x.Item.Id == Id).FirstOrDefault();
                if (item != null)
                {
                    ScroolTo(item);
                    ScrolledToId = true;
                }
            }
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

        public override void PerformSearch(string newText)
        {
            if (
                ItemsList != null
                &&
                ItemsList.Count > 0
                )
            {
                if (!string.IsNullOrWhiteSpace(newText))
                {
                    Task.Run(() =>
                    {
                        var itemsResult = PerformSearchSetup(newText);

                        MainThread.BeginInvokeOnMainThread(() =>
                        {
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

                        });
                        
                    });

                }
                else
                {
                    ItemsSource = ItemsList;
                    HideMessage();

                }
            }
            else
            { 
                
                if (!_isLoadingData)
                {
                    ShowMessage(GetEmptyText(), true, true);
                    ItemsSource = ItemsList;
                }
                else
                {
                    ShowMessage(GetLoadingText());
                }

            }
        }

        public virtual void SubscribeListViewItemAppearing()
        {
            var message = string.Format("ListView_ItemAppearing {0}", _uuidMessagingCenter);
            MessagingCenter.Subscribe<object, object>(this, message, (sender, args) =>
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
            var message = string.Format("SearchBar_Focused {0}", _uuidMessagingCenter);

            MessagingCenter.Subscribe<object, bool>(this, message, (sender, isFocused) =>
            {
                SearchBarFocused(isFocused);
            });
        }

        public virtual void SubscribeScroolToAnimate()
        {
            var message = string.Format("ListView_ScroolToItem_Animate_To_ViewModel {0}", _uuidMessagingCenter);
            MessagingCenter.Subscribe<ContentView>(this, message, (sender) =>
            {
                ScroolToAnimate(sender);
            });

        }

        public virtual void UnsubscribeListViewItemAppearing()
        {
            var message = string.Format("ListView_ItemAppearing {0}", _uuidMessagingCenter);
            MessagingCenter.Unsubscribe<object, object>(this, message);
        }

        public virtual void UnsubscribeSearchBarFocused()
        {
            var message = string.Format("SearchBar_Focused {0}", _uuidMessagingCenter);
            MessagingCenter.Unsubscribe<object, bool>(this, message);
        }

        public virtual void UnsubscribeScroolToAnimate()
        {
            var message = string.Format("ListView_ScroolToItem_Animate_To_ViewModel {0}", _uuidMessagingCenter);
            MessagingCenter.Unsubscribe<ContentView, object>(this, message);
        }

        public virtual void SubscribeMessagingCenter()
        {
            SubscribeListViewItemAppearing();
            SubscribeSearchBarFocused();
            SubscribeScroolToAnimate();
        }

        public virtual void UnsubscribeMessagingCenter()
        {
            UnsubscribeListViewItemAppearing();
            UnsubscribeSearchBarFocused();
            UnsubscribeScroolToAnimate();
        }

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
                ScroolTo(cellVM);
            }

        }

        public virtual void ScroolTo(object listObj)
        {
            var scroolToItem = new ScroolToItem()
            {
                IsAnimate = ScroolToAnimateIsEnable(),
                Item = listObj
            };

            var message = string.Format("ListView_OnScrool {0}", _uuidMessagingCenter);
            MessagingCenter.Send(this as object, message, scroolToItem);
        }

        public virtual bool ScroolToAnimateIsEnable()
        {
            return true;
        }

        public virtual void ScroolToAnimate(ContentView contentView)
        {
            System.Diagnostics.Debug.WriteLine("ANIMO");

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(500);

                await contentView.ScaleTo(1.08, 2000, Easing.BounceIn);
                await contentView.ScaleTo(1, 250, Easing.BounceOut);
            });

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

        public override async void TlbSendClick()
        {
            var l = new List<TItem>();

            foreach (var cellVM in ItemsList.Where(x => x.IsSelected).ToList())
            {
                l.Add(cellVM?.Item);
            }

            var navParams = new NavigationParameters
            {
                { "selectedItems", l}
            };

            var navResult = await NavigationService.GoBackAsync(navParams, useModalNavigation: true);
        }
    }

    public class ScroolToItem
    {
        public object Item { get; set; }
        public bool IsAnimate { get; set; }
    }

    
}
