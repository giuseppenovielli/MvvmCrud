using MVVMCrud.Models.Base;
using MVVMCrud.Models.Request;

namespace MVVMCrud.Utils.Request
{
    public class RequestItems<TItemsRoot, TItem> : BaseRequest<TItemsRoot>
        where TItemsRoot : BaseModelItemsRoot<TItem>
        where TItem : BaseItem, new()
    {
        public RequestItems(
            string urlPagination = null,
            RequestConfig requestConfig = null) : base(requestConfig)
        {
            RequestConfig.ItemSetup = null;
            RequestConfig.ItemSetup = () => { SetupPagination(urlPagination); };
        }

        public virtual void SetupPagination(string urlPagination)
        {  
        }
    }
}
