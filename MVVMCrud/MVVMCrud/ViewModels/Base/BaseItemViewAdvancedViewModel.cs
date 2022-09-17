using System.Threading.Tasks;
using Prism.Navigation;
using MVVMCrud.Models.Base;
using MVVMCrud.Services.Request;
using System;

namespace MVVMCrud.ViewModels.Base
{
    public class BaseItemViewAdvancedViewModel<TItem, TItemRoot> : BaseItemEditViewModel
        where TItem : BaseItem, new()
        where TItemRoot : BaseModelItemRoot<TItem>, new()
    {
        public string Id { get; set; }
        public long IdLong { get; set; }

        public TItem Item { get; set; }

        public BaseItemViewAdvancedViewModel(
            INavigationService navigationService,
            IRequestService requestService) : base(navigationService, requestService)
        {
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters.ContainsKey("id"))
            {
                Id = parameters.GetValue<string>("id");

                try
                {
                    IdLong = Convert.ToInt64(Id);
                }
                catch (System.Exception) { }

                _ = GetItem();
            }
        }

        public override void PageRefresh()
        {
            base.PageRefresh();

            _ = GetItem();
        }

        public async virtual Task<TItemRoot> SetupRequest()
        {
            if (!string.IsNullOrWhiteSpace(Endpoint))
            {
                return await RequestService.RequestDefaultRetrieveItem<TItemRoot, TItem>(Endpoint, Id.ToString(), TitlePage);
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
