using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MVVMCrud.Models.Base;
using MVVMCrud.Models.ItemRoot;

namespace MVVMCrud.Services.Request
{
    public interface IRequestService
    {
        #region BaseRequest
        Task<TItemRoot> RequestItem<TItemRoot, TItem>
            (Func<Task<TItemRoot>> request,
            string title = null,
            bool getIfError = false,
            bool showLoading = false,
            List<HttpStatusCode> httpStatusCodes = null,
            Action setupItem = null)

            where TItemRoot : BaseModelItemRoot<TItem>
            where TItem : BaseItem, new();



        Task<TItemsRoot> RequestItems<TItemsRoot, TItem>
            (Func<string, bool, Task<TItemsRoot>> request,
            string title,
            Action<string> paginationSetup = null,
            string urlPagination = null,
            bool getIfError = false,
            bool pagination = true,
            List<HttpStatusCode> httpStatusCodes = null)

            where TItemsRoot : BaseModelItemsRoot<TItem>
            where TItem : BaseItem, new();



        Task<RootItemBase> RequestDeleteItem
            (Func<Task<RootItemBase>> request,
            string title = null,
            bool getIfError = false,
            bool showLoading = true,
            List<HttpStatusCode> httpStatusCodes = null,
            Action setupItem = null);

        #endregion

        #region Default Request
        Task<RootItemBase> RequestDefaultDeleteItem
            (string url,
            string pk,
            string title = null,
            HttpClient httpClient = null,
            bool getIfError = false,
            bool showLoading = true,
            List<HttpStatusCode> httpStatusCodes = null,
            Action setupItem = null);


        Task<TItemsRoot> RequestDefaultListItems<TItemsRoot, TItem>
            (string url,
            string title = null,
            bool extra = false,
            FormUrlEncodedContent contentquery = null,
            HttpClient httpClient = null,
            Action<string> paginationSetup = null,
            string urlPagination = null,
            bool getIfError = false,
            bool pagination = true,
            List<HttpStatusCode> httpStatusCodes = null)

            where TItemsRoot : BaseModelItemsRoot<TItem>, new()
            where TItem : BaseItem, new();



        Task<TItemRoot> RequestDefaultCreateUpdateItem<TItemRoot, TItem>
            (string url,
            MultipartFormDataContent formDataContent = null,
            HttpContent httpContent = null,
            string pk = null,
            string title = null,
            HttpClient httpClient = null,
            bool getIfError = false,
            bool showLoading = true,
            List<HttpStatusCode> httpStatusCodes = null,
            Action setupItem = null)

            where TItemRoot : BaseModelItemRoot<TItem>, new()
            where TItem : BaseItem, new();


        Task<TItemRoot> RequestDefaultRetrieveItem<TItemRoot, TItem>
            (string url,
            string pk,
            string title = null,
            HttpClient httpClient = null,
            bool getIfError = false,
            bool showLoading = false,
            List<HttpStatusCode> httpStatusCodes = null,
            Action setupItem = null,
            FormUrlEncodedContent contentquery = null)

            where TItemRoot : BaseModelItemRoot<TItem>, new()
            where TItem : BaseItem, new();
        #endregion

        #region API ROOT
        Task<TItemsRoot> List<TItemsRoot, TItem>
            (string url,
            FormUrlEncodedContent contentquery = null,
            string urlPagination = null, bool pagination = true, bool extra = false,
            HttpClient httpClient = null)

            where TItemsRoot : BaseModelItemsRoot<TItem>, new()
            where TItem : BaseItem, new();


        Task<TItemRoot> Retrieve<TItemRoot, TItem>
            (string url,
            string pk = null,
            HttpClient httpClient = null,
            FormUrlEncodedContent contentquery = null)

            where TItemRoot : BaseModelItemRoot<TItem>, new()
            where TItem : BaseItem, new();


        Task<RootItemBase> Delete
            (string url,
            string pk = null,
            HttpClient httpClient = null);


        Task<TItemRoot> CreateUpdate<TItemRoot, TItem>
            (string url,
            string pk = null,
            MultipartFormDataContent formData = null,
            HttpContent httpContent = null,
            HttpClient httpClient = null)

            where TItemRoot : BaseModelItemRoot<TItem>, new()
            where TItem : BaseItem, new();

        #endregion
    }
}
