using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using MVVMCrud.Controls;
using MVVMCrud.CustomView;
using MVVMCrud.Models.Base;
using MVVMCrud.Models.ItemRoot;
using MVVMCrud.Services.Request;
using MVVMCrud.Utils;
using Newtonsoft.Json;
using Prism.Navigation;

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

        public string Id { get; private set; }
        public bool ScrolledToId { get; set; }

        readonly string _uuidMessagingCenter;
        bool _isLoadingData;

        public BaseListPaginationAdvancedViewModel(
            INavigationService navigationService,
            IRequestService requestService,
            bool buttonRefreshIsVisible = false) : base(navigationService, requestService)
        {
            _uuidMessagingCenter = MVVMCrudApplication.GetLastPageUUID();
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            SubscribeMessagingCenter();

            TitlePage = SetupTitlePage();
            Endpoint = SetupEndpoint();

            ShowMessage(MVVMCrudApplication.GetLoadingText());

            if (string.IsNullOrWhiteSpace(Id))
            {
                _ = SetupGet();
            }

        }

        public override void InitializeParameters(INavigationParameters parameters)
        {
            base.InitializeParameters(parameters);

            if (parameters.ContainsKey("id")) { Id = parameters.GetValue<string>("id"); }
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            if (!string.IsNullOrWhiteSpace(Id)
                &&
                Start)
            {
                await SetupGet();
                Start = false;
            }
        }

        public override string SetupTitlePage()
        {
            var pageName = Utils.Utils.GetPageNameWithUnderscore(GetType().Name, "PageViewModel");
            var label = string.Format("title_page_{0}", pageName);
            return MVVMCrudApplication.GetAppResourceManager().GetString(label);
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

        public virtual bool GetItemsWithExtra()
        {
            return false;
        }


        public virtual async Task<TItemsRoot> SetupGetItemsRequest(string urlPagination = null, bool pagination = true)
        {
            if (!string.IsNullOrWhiteSpace(Endpoint))
            {
                return await RequestService.List<TItemsRoot, TItem>(Endpoint, SetupContentQuery(), urlPagination, pagination, GetItemsWithExtra(), GetHttpClient());
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
                urlPagination,
                SetupGetResponseItemIfError(),
                pagination);


            if (rootItemBase != null)
            {
                PaginationItem = rootItemBase.PaginationItem;
                GetItemsSetupList(rootItemBase);
            }

            SetupLoadingComplete();
        }

        public virtual bool SetupGetResponseItemIfError()
        {
            return false;
        }

        public virtual void SetupLoadingComplete()
        {
            _isLoadingData = false;

            ProcessPagination();

            SetupLoadingCompleteItems();

            EndLoadingMore();

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

            var navResult = await NavigationService.GoBackAsync(navParams);

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
            var page = SetupCreateUpdatePage();
            var navParams = SetupEditItemNavParams(cellVM);
            var useModalNavigation = SetupCreateUpdatePageIsModal();

            var navResult = await SetupNavigationPage(page, navParams, cellVM, useModalNavigation);
        }

        public virtual NavigationParameters SetupEditItemNavParams(TCellVM cellVM)
        {
            return new NavigationParameters
            {
                { "endpoint", SetupCreateUpdatePageEndpoint() },
                { "itemSerialized", SetupCreateUpdatePageItemSerialized(cellVM) },
                { "position", ItemsList.IndexOf(cellVM) }
            };
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
            //var result = await NavigationService.CreateBuilder()
            //    .UseAbsoluteNavigation(SetupCreateUpdatePageIsModal())
            //    .AddSegment(nameof(MVVMCrudModalNavigationPage))
            //    .AddSegment(page, SetupCreateUpdatePageIsModal())
            //    .WithParameters(navParams)
            //    .NavigateAsync();

            //if (SetupCreateUpdatePageIsModal())
            //{
            //    navParams.Add(KnownNavigationParameters.UseModalNavigation, true);
            //}

            var navParams = SetupCreateNavParams();
            var page = SetupCreateUpdatePage();
            var useModalNavigation = SetupCreateUpdatePageIsModal();
            var navResult = await SetupNavigationPage(page, navParams, useModalNavigation: useModalNavigation);
        }

        public virtual NavigationParameters SetupCreateNavParams()
        {
            return new NavigationParameters
            {
                { "endpoint", SetupCreateUpdatePageEndpoint() },
            };
        }


        public virtual async Task<object> SetupNavigationPage(string page, INavigationParameters navParams, TCellVM cellVM = null, bool useModalNavigation = false)
        {
            return await NavigationService.NavigateAsync(page, navParams);
            //return await NavigationService.NavigateAsync(page, navParams, useModalNavigation: useModalNavigation);
        }

        public virtual string SetupCreateUpdatePageName(TCellVM cellVM = null)
        {
            return GetType().Name;
        }

        public virtual string SetupCreateUpdatePage(TCellVM cellVM = null)
        {
            var pageVMName = SetupCreateUpdatePageName(cellVM);

            var pageContext = pageVMName.Split(new[] { "PageViewModel" }, StringSplitOptions.None);
            if (pageContext?.Length == 2)
            {
                var pageName = pageContext[0];

                var pageNewEdit = pageName + "NewEditPage";

                return pageNewEdit;

                //if (!SetupCreateUpdatePageIsModal(cellVM)
                //{
                //    return pageNewEdit;
                //}
                //else
                //{
                //    return string.Format("{0}/{1}", nameof(MVVMCrudModalNavigationPage), pageNewEdit);
                //}


            }

            return null;
        }

        public virtual bool SetupCreateUpdatePageIsModal(TCellVM cellVM = null)
        {
            if (DeviceInfo.Platform == DevicePlatform.iOS)
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

        public virtual string SetupDetailPageName(TCellVM obj)
        {
            return string.Empty;
        }

        public virtual bool IsDetailPageWithHeader(TCellVM obj)
        {
            return false;
        }

        public virtual string SetupDetailPageID(TCellVM obj)
        {
            return obj.Item.Id;
        }

        public virtual bool IsDetailPageModal(TCellVM obj)
        {
            return false;
        }

        public virtual async void SetupDetailPage(TCellVM obj)
        {
            var navResult = await SetupNavigationPage(SetupDetailPageName(obj), SetupDetailPageNavParams(obj), obj, IsDetailPageModal(obj));
        }

        public virtual NavigationParameters SetupDetailPageNavParams(TCellVM obj)
        {
            var withHeader = IsDetailPageWithHeader(obj);
            var id = SetupDetailPageID(obj);
            
            var position = ItemsSource.IndexOf(obj);
            var fromPageName = GetType().Name;

            var navParams = new NavigationParameters
            {
                { "position", position },
                { "fromPageViewModelName", fromPageName},
            };

            if (withHeader)
            {
                navParams.Add("headerId", id);
                navParams.Add("headerEndpoint", Endpoint);
            }
            else
            {
                navParams.Add("id", id);
            }

            return navParams;
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
            //var message = string.Format("ListView_ItemAppearing {0}", _uuidMessagingCenter);
            //MessagingCenter.Subscribe<object, object>(this, message, (sender, args) =>
            //{
            //    if (
            //        ItemsList != null
            //        &&
            //        ItemsList.Count > 0
            //        &&
            //        args == ItemsList[ItemsList.Count - 1]
            //    )
            //    {
            //        ItemsThresholdReached();
            //    }

            //});

            // Register a message in some module
            WeakReferenceMessenger.Default.Register<ListViewItemAppearingChangeMessage>(this, (r, m) =>
            {
                if (
                        ItemsList != null
                        &&
                        ItemsList.Count > 0
                        &&
                        m?.Uuid == _uuidMessagingCenter
                        &&
                        m?.Value == ItemsList[ItemsList.Count - 1]
                    )
                {
                    ItemsThresholdReached();
                }
            });
        }

        public virtual void SubscribeSearchBarFocused()
        {
            //var message = string.Format("SearchBar_Focused {0}", _uuidMessagingCenter);
            //MessagingCenter.Subscribe<object, bool>(this, message, (sender, isFocused) =>
            //{
            //    SearchBarFocused(isFocused);
            //});

            // Register a message in some module
            WeakReferenceMessenger.Default.Register<SearchBarFocusedMessage>(this, (r, m) =>
            {
                if (
                        m?.Value != null
                        &&
                        m?.Uuid == _uuidMessagingCenter
                    )
                {
                    SearchBarFocused(m.Value);
                }
                
            });
        }

        public virtual void SubscribeScroolToAnimate()
        {
            //var message = string.Format("ListView_ScroolToItem_Animate_To_ViewModel {0}", _uuidMessagingCenter);
            //MessagingCenter.Subscribe<ContentView>(this, message, (sender) =>
            //{
            //    ScroolToAnimate(sender);
            //});

            WeakReferenceMessenger.Default.Register<ListViewScroolToItemAnimateToViewModelMessage>(this, (r, m) =>
            {
                if (
                        m?.Value != null
                        &&
                        m?.Uuid == _uuidMessagingCenter
                    )
                {
                    ScroolToAnimate(m.Value);
                }
            });

        }

        public virtual void UnsubscribeListViewItemAppearing()
        {
            //var message = string.Format("ListView_ItemAppearing {0}", _uuidMessagingCenter);
            //MessagingCenter.Unsubscribe<object, object>(this, message);

            WeakReferenceMessenger.Default.Unregister<ListViewItemAppearingChangeMessage>(this);
        }

        public virtual void UnsubscribeSearchBarFocused()
        {
            //var message = string.Format("SearchBar_Focused {0}", _uuidMessagingCenter);
            //MessagingCenter.Unsubscribe<object, bool>(this, message);

            WeakReferenceMessenger.Default.Unregister<SearchBarFocusedMessage>(this);
        }

        public virtual void UnsubscribeScroolToAnimate()
        {
            //var message = string.Format("ListView_ScroolToItem_Animate_To_ViewModel {0}", _uuidMessagingCenter);
            //MessagingCenter.Unsubscribe<ContentView, object>(this, message);

            WeakReferenceMessenger.Default.Unregister<ListViewScroolToItemAnimateToViewModelMessage>(this);
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

            if (parameters.ContainsKey("deleteItem"))
            {
                var pos = parameters.GetValue<int>("deleteItem");
                SetupDeleteItem(pos);
            }
            else
            {
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
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            if (parameters.GetNavigationMode() == Prism.Navigation.NavigationMode.Back)
            {
                UnsubscribeMessagingCenter();
            }
            
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

            //var message = string.Format("ListView_OnScrool {0}", _uuidMessagingCenter);
            //MessagingCenter.Send(this as object, message, scroolToItem);

            // Send a message from some other module
            WeakReferenceMessenger.Default.Send(new ListViewScroolToMessage(_uuidMessagingCenter, scroolToItem));
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
                await Task.Delay(200);

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

            var navResult = await NavigationService.GoBackAsync(navParams);
        }
    }

    public class ScroolToItem
    {
        public object Item { get; set; }
        public bool IsAnimate { get; set; }
    }


}
