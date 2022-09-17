using MVVMCrud.Models.Base;
using MVVMCrud.Models.Request;

namespace MVVMCrud.Utils.Request
{
    public class RequestItem<TItemsRoot, TItem> : BaseRequest<TItemsRoot>
        where TItemsRoot : BaseModelItemRoot<TItem>
        where TItem : BaseItem, new()
    {
        public RequestItem(RequestConfig requestConfig = null) : base(requestConfig)
        {
        }
    }
}
