using System.Threading.Tasks;
using Prism.Navigation;
using MVVMCrud.Models.Base;
using MVVMCrud.Services.Request;
using System;
using MVVMCrud.Utils;
using System.Net.Http;

namespace MVVMCrud.ViewModels.Base
{
    public class BaseItemViewAdvancedViewModel<TItem, TItemRoot> : BaseItemEditViewModel
        where TItem : BaseItem, new()
        where TItemRoot : BaseModelItemRoot<TItem>, new()
    {
        public int Position { get; private set; }
        public string Id { get; private set; }
        public long IdLong { get; private set; }

        public TItem Item { get; set; }

        public BaseItemViewAdvancedViewModel(
            INavigationService navigationService,
            IRequestService requestService) : base(navigationService, requestService)
        {
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            TitlePage = SetupTitlePage();
            Endpoint = SetupEndpoint();

            if (!string.IsNullOrWhiteSpace(Id))
            {
                _ = GetItem();
            }
        }

        public override void InitializeParameters(INavigationParameters parameters)
        {
            base.InitializeParameters(parameters);

            if (parameters.ContainsKey("id"))
            {
                Id = parameters.GetValue<string>("id");

                try
                {
                    IdLong = Convert.ToInt64(Id);
                }
                catch (System.Exception) { }

                if (parameters.ContainsKey("position"))
                {
                    Position = parameters.GetValue<int>("position");
                }

                if (parameters.ContainsKey("endpoint"))
                {
                    Endpoint = parameters.GetValue<string>("endpoint");
                }
            }


        }

        public override string SetupEndpoint()
        {
            return Endpoint;
        }

        public override void PageRefresh()
        {
            base.PageRefresh();

            _ = GetItem();
        }

        public virtual FormUrlEncodedContent SetupContentQuery()
        {
            return null;
        }

        public async virtual Task<TItemRoot> SetupRequest()
        {
            if (!string.IsNullOrWhiteSpace(Endpoint))
            {
                return await RequestService.Retrieve<TItemRoot, TItem>(Endpoint, Id.ToString(), GetHttpClient(), SetupContentQuery());
            }

            return await Task.FromResult<TItemRoot>(null);
        }

        public virtual string GetEmptyText()
        {
            return MVVMCrudApplication.GetEmptyText();
        }

        public virtual void SetupLoadingComplete()
        {
            if (Item != null)
            {

                SetupInterface();
                HideMessage();
            }
            else
            {
                ShowMessage(GetEmptyText(), true);
            }

        }

        public virtual Task SetupOtherItems(BaseModelItemRoot<TItem> rootItem)
        {
            return Task.CompletedTask;
        }

        public virtual void SetupInterface()
        {
            TitlePage = SetupTitlePage();

        }

        public override string SetupTitlePage()
        {
            var pageName = Utils.Utils.GetPageNameWithUnderscore(GetType().Name, "PageViewModel");
            var label = string.Format("title_activity_{0}", pageName);
            var name = MVVMCrudApplication.GetAppResourceManager().GetString(label);
            return name;
        }

        public async Task GetItem()
        {
            var rootItem = await RequestService.RequestItem<TItemRoot, TItem>
                (SetupRequest, TitlePage, setupItem: SetupItem);

            await SetupOtherItems(rootItem);

            if (rootItem?.Item != null)
            {
                Item = rootItem.Item;
            }

            SetupLoadingComplete();
        }

        public virtual string GetLoadingText()
        {
            return MVVMCrudApplication.GetLoadingText();
        }

        public virtual void SetupItem()
        {
            Item = null;
            ShowMessage(GetLoadingText());
        }
    }
}
