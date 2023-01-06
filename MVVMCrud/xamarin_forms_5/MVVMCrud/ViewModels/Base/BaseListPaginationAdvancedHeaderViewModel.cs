using System;
using System.Threading.Tasks;
using MVVMCrud.CustomView;
using MVVMCrud.Models.Base;
using MVVMCrud.Models.ItemRoot;
using MVVMCrud.Services.Request;
using MVVMCrud.Utils;
using MVVMCrud.Views.Base;
using Newtonsoft.Json;
using Prism.Navigation;
using Xamarin.Forms;

namespace MVVMCrud.ViewModels.Base
{
    public class BaseListPaginationAdvancedHeaderViewModel
        <
        TCellVM, TItemsRoot, TItem,
        THeaderCellVM, THeaderItemRoot, THeaderItem
        >

        :

        BaseListPaginationAdvancedViewModel<TCellVM, TItemsRoot, TItem>

        where TItem : BaseItem, new()
        where TCellVM : BaseCellViewModel<TItem>, new()
        where TItemsRoot : BaseModelItemsRoot<TItem>, new()

        where THeaderItem : BaseItem, new()
        where THeaderCellVM : BaseCellViewModel<THeaderItem>, new()
        where THeaderItemRoot : BaseModelItemRoot<THeaderItem>, new()
        
    {

        public string HeaderEndpoint { get; set; }
        public string HeaderID { get; set; }
        public string HeaderPosition { get; set; }
        public string FromPageViewModelName { get; set; }

        public THeaderCellVM HeaderVM { get; set; }
        public BaseContentView HeaderView { get; set; }

        public NewEditItem<THeaderItem> HeaderEdited { get; private set; }

        public BaseListPaginationAdvancedHeaderViewModel(
            INavigationService navigationService,
            IRequestService requestService) : base(navigationService, requestService)
        {
        }

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("id"))
            {
                HeaderID = parameters.GetValue<string>("id");
            }

            if (parameters.ContainsKey("position"))
            {
                HeaderPosition = parameters.GetValue<string>("position");
            }

            FromPageViewModelName = SetupFromPageViewModelName();

            base.Initialize(parameters);
        }

        public virtual string SetupHeaderEndpoint()
        {
            return Endpoint;
        }

        public virtual string SetupFromPageViewModelName()
        {
            return string.Empty;
        }

        public override string SetupTitlePage()
        {
            return string.Empty;
        }

        public override async Task SetupGet()
        {
            HeaderEndpoint = SetupHeaderEndpoint();
            await GetHeader();
        }

        public async Task GetHeader()
        {
            var rootItemBase = await RequestService.RequestDefaultRetrieveItem<THeaderItemRoot, THeaderItem>
                (
                    HeaderEndpoint,
                    HeaderID,
                    TitlePage,
                    setupItem: SetupGetHeaderInitialize
                );


            GetHeaderItemSetupList(rootItemBase);
            await SetupHeaderLoadingComplete();
        }

        public virtual void SetupGetHeaderInitialize()
        {
            ShowMessage(GetLoadingText());
            HeaderView = null;
            HeaderVM = null;
        }

        public virtual BaseContentView SetupHeaderView()
        {
            return new BaseContentView();
        }

        public virtual async Task SetupHeaderLoadingComplete()
        {
            if (HeaderVM != null)
            {
                await GetItems(pagination: SetupIsPaginationEnable());
            }
            
        }

        public virtual void SetupHeaderItemExtra(THeaderItemRoot obj)
        {
        }

        public override void SetupLoadingComplete()
        {
            if (HeaderView == null)
            {
                HeaderView = SetupHeaderView();
                HeaderView.BindingContext = HeaderVM;
            }

            base.SetupLoadingComplete();

        }

        void GetHeaderItemSetupList(THeaderItemRoot obj)
        {
            SetupHeaderItemExtra(obj);

            if (obj != null)
            {
                HeaderVM = InstanceHeaderCellVM(obj.Item);                
            }
        }


        THeaderCellVM InstanceHeaderCellVM(THeaderItem item)
        {
            if (item != null)
            {
                var cellVM = SetupHeaderInstanceCell(item);
                if (cellVM != null)
                {
                    cellVM.DeleteClickCommand = new Command<THeaderCellVM>(async (obj) =>
                    {
                        SetupHeaderOnDeletedItem(obj, await SetupHeaderUploadDeleteItem(obj));
                    });

                    cellVM.EditClickCommand = new Command<THeaderCellVM>((obj) =>
                    {
                        SetupHeaderEditItem(obj);

                    });

                    return cellVM;
                }
            }

            return null;
        }

        public virtual async void SetupHeaderEditItem(THeaderCellVM cellVM)
        {
            var navParams = new NavigationParameters
            {
                { "endpoint", HeaderEndpoint },
                { "itemSerialized", SetupHeaderCreateUpdatePageItemSerialized(cellVM) },
            };

            var page = SetupHeaderCreateUpdatePage();
            if (!string.IsNullOrWhiteSpace(page))
            {
                var navResult = await NavigationService.NavigateAsync(page, navParams, useModalNavigation: SetupCreateUpdatePageIsModal());
            }

        }

        public virtual string SetupHeaderCreateUpdatePageName()
        {
            return FromPageViewModelName;
        }

        public virtual string SetupHeaderCreateUpdatePage()
        {
            var pageVMName = SetupHeaderCreateUpdatePageName();

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

        public virtual string SetupHeaderCreateUpdatePageItemSerialized(THeaderCellVM cellVM)
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

        public virtual THeaderCellVM SetupHeaderInstanceCell(THeaderItem item)
        {
            var cell = new THeaderCellVM();
            cell.Initialize(item);
            return cell;
        }

        public virtual async Task<RootItemBase> SetupHeaderUploadDeleteItem(THeaderCellVM cellVM)
        {
            var id = cellVM?.Item?.Id;
            if (!string.IsNullOrWhiteSpace(id)
                &&
                await SetupDeleteMessageConfirm())
            {
                return await RequestService.RequestDefaultDeleteItem(SetupDeleteHeaderEndpoint(), id, TitlePage);
            }

            return null;
        }

        public virtual string SetupDeleteHeaderEndpoint()
        {
            return HeaderEndpoint;
        }

        public virtual async void SetupHeaderOnDeletedItem(THeaderCellVM cellVM, RootItemBase rootItemBase)
        {
            if (rootItemBase != null
                &&
                SetupHeaderDeletedItem(cellVM))
            {
                var navParams = new NavigationParameters
                {
                    { "deleteItem", HeaderPosition }
                };
                var navResult = await NavigationService.GoBackAsync(navParams);
            }
        }

        public virtual bool SetupHeaderDeletedItem(THeaderCellVM cellVM)
        {
            HeaderVM = null;
            HeaderView = null;
            return true;
        }

        public virtual void SetupDefaultHeaderDeleteItemMessage()
        {
            SetupDefaultDeleteItemMessage();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("newEditItem"))
            {
                var editHeaderItem = parameters["newEditItem"] as NewEditItem<THeaderItem>;
                if (editHeaderItem != null)
                {
                    var pos = editHeaderItem.Position;
                    if (pos > -1)
                    {
                        //Edit
                        HeaderEdited = editHeaderItem;
                        UpdateEditHeaderItem(editHeaderItem);
                    }
                }
            }
        }

        public virtual void UpdateEditHeaderItem(NewEditItem<THeaderItem> editHeaderItem)
        {
            var pos = editHeaderItem.Position;
            var section = editHeaderItem.Section;

            if (pos < ItemsList.Count)
            {
                HeaderVM = InstanceHeaderCellVM(editHeaderItem.Item);
                if (HeaderVM != null)
                {
                    SetupEditItemMessage();
                    HeaderView.BindingContext = HeaderVM;

                }
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            if (parameters.GetNavigationMode() == NavigationMode.Back)
            {
                if (HeaderEdited != null)
                {
                    parameters.Add("newEditItem", HeaderEdited);
                }
                
            }
        }
    }
}
