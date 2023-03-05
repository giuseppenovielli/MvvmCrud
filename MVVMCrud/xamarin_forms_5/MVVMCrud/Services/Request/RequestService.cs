using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MVVMCrud.Models.Base;
using MVVMCrud.Models.ItemRoot;
using MVVMCrud.Models.RequestResponse;
using MVVMCrud.Services.RequestProvider;
using MVVMCrud.Utils.Request;

namespace MVVMCrud.Services.Request
{
    public class RequestService : IRequestService
    {
        private readonly IRequestProvider _requestProvider;

        public RequestService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        #region BaseRequest
        public async Task<TItemRoot> RequestItem<TItemRoot, TItem>
            (Func<Task<TItemRoot>> request,
            string title = null,
            bool getIfError = false,
            bool showLoading = false,
            List<HttpStatusCode> httpStatusCodes = null,
            Action setupItem = null)

            where TItemRoot : BaseModelItemRoot<TItem>
            where TItem : BaseItem, new()
        {
            var requestSetupResponse = MVVMCrudApplication.Instance?.SetupBaseRequestSetupResponse();
            if (requestSetupResponse == null)
            {
                requestSetupResponse = new BaseRequestSetupResponse();
            }
            requestSetupResponse.Initialize(title);

            if (Utils.Utils.IsConnected())
            {
                setupItem?.Invoke();

                if (showLoading)
                {
                    await requestSetupResponse.SetupShowLoading();
                }

                var rootItemBase = await request();
                if (requestSetupResponse.Analize(rootItemBase, httpStatusCodes, getIfError))
                {
                    if (showLoading) { await requestSetupResponse.SetupHideLoading(); }
                    return rootItemBase;
                }

                if (showLoading){ await requestSetupResponse.SetupHideLoading();}
            }
            else
            {
                requestSetupResponse.SetupResponseNoConnection(title);
            }

            return null;
        }

        public async Task<TItemsRoot> RequestItems<TItemsRoot, TItem>
            (Func<string, bool, Task<TItemsRoot>> request,
            string title,
            Action<string> paginationSetup = null,
            string urlPagination = null,
            bool getIfError = false,
            bool pagination = true,
            List<HttpStatusCode> httpStatusCodes = null)

            where TItemsRoot : BaseModelItemsRoot<TItem>
            where TItem: BaseItem, new()
        {
            var requestSetupResponse = MVVMCrudApplication.Instance?.SetupBaseRequestSetupResponse();
            if (requestSetupResponse == null)
            {
                requestSetupResponse = new BaseRequestSetupResponse();
            }
            requestSetupResponse.Initialize(title);

            if (Utils.Utils.IsConnected())
            {
                paginationSetup?.Invoke(urlPagination);

                var rootItemBase = await request(urlPagination, pagination);

                if (requestSetupResponse.Analize(rootItemBase, httpStatusCodes, getIfError))
                {
                    return rootItemBase;
                }

            }
            else
            {
                requestSetupResponse.SetupResponseNoConnection(title);
            }

            return null;
        }

        public async Task<RootItemBase> RequestDeleteItem
            (Func<Task<RootItemBase>> request,
            string title = null,
            bool getIfError = false,
            bool showLoading = true,
            List<HttpStatusCode> httpStatusCodes = null,
            Action setupItem = null)
        {
            var requestSetupResponse = MVVMCrudApplication.Instance?.SetupBaseRequestSetupResponse();
            if (requestSetupResponse == null)
            {
                requestSetupResponse = new BaseRequestSetupResponse();
            }
            requestSetupResponse.Initialize(title);

            if (Utils.Utils.IsConnected())
            {
                if (httpStatusCodes == null)
                {
                    httpStatusCodes = new List<HttpStatusCode>() { HttpStatusCode.NoContent, HttpStatusCode.OK };
                }
                setupItem?.Invoke();

                if (showLoading)
                {
                    await requestSetupResponse.SetupShowLoading();
                }

                var rootItemBase = await request();


                var baseModelRoot = new BaseModelRoot
                {
                    RootItemBase = rootItemBase
                };

                if (requestSetupResponse.Analize(baseModelRoot, httpStatusCodes, getIfError))
                {
                    if (showLoading) { await requestSetupResponse.SetupHideLoading(); }
                    return rootItemBase;
                }

                if (showLoading) { await requestSetupResponse.SetupHideLoading(); }
            }
            else
            {
                requestSetupResponse.SetupResponseNoConnection(title);
            }

            return null;
        }
        #endregion

        #region DefaultRequest
        public async Task<RootItemBase> RequestDefaultDeleteItem
            (string url,
            string pk,
            string title = null,
            HttpClient httpClient = null,
            bool getIfError = false,
            bool showLoading = true,
            List<HttpStatusCode> httpStatusCodes = null,
            Action setupItem = null)
        {
            var httpCodes = httpStatusCodes;
            if (httpStatusCodes == null)
            {
                httpCodes = new List<HttpStatusCode>() { HttpStatusCode.NoContent };
            }

            return await RequestDeleteItem(
                async () =>
                {
                    return await Delete(url, pk, httpClient);
                },
                title, getIfError, showLoading, httpStatusCodes, setupItem);
        }




        public async Task<TItemsRoot> RequestDefaultListItems<TItemsRoot, TItem>
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
            where TItem : BaseItem, new()
        {
            return await RequestItems<TItemsRoot, TItem>(

                async (urlPaginationLocal, paginationLocal) =>
                {
                    return await List<TItemsRoot, TItem>(url, contentquery, urlPaginationLocal, paginationLocal, extra, httpClient);
                },
                title, paginationSetup, urlPagination, getIfError, pagination, httpStatusCodes);
        }



        public async Task<TItemRoot> RequestDefaultCreateUpdateItem<TItemRoot, TItem>
            (string url,
            MultipartFormDataContent formDataContent = null,
            HttpContent httpContent = null,
            string pk = null,
            string title = null,
            HttpClient httpClient = null,
            bool getIfError = false,
            bool showLoading = true,
            List<HttpStatusCode> httpStatusCodes = null,
            Action setupItem = null,
            bool partialUpdate = false)

            where TItemRoot : BaseModelItemRoot<TItem>, new()
            where TItem : BaseItem, new()
        {
            var httpCodes = httpStatusCodes;
            if (httpStatusCodes == null
                &&
                string.IsNullOrWhiteSpace(pk))
            {
                httpCodes = new List<HttpStatusCode>() { HttpStatusCode.Created };
            }

            return await RequestItem<TItemRoot, TItem>(
                async () =>
                {
                    return await CreateUpdate<TItemRoot, TItem>(url, pk, formDataContent, httpContent, httpClient, partialUpdate);
                }, title, getIfError, showLoading, httpCodes, setupItem);
        }




        public async Task<TItemRoot> RequestDefaultRetrieveItem<TItemRoot, TItem>
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
            where TItem : BaseItem, new()
        {

            return await RequestItem<TItemRoot, TItem>(
                async () =>
                {
                    return await Retrieve<TItemRoot, TItem>(url, pk, httpClient, contentquery);
                }, title, getIfError, showLoading, httpStatusCodes, setupItem);
        }


        #endregion

        #region API ROOT
        public async Task<TItemsRoot> List<TItemsRoot, TItem>
            (string url,
            FormUrlEncodedContent contentquery = null,
            string urlPagination = null,
            bool pagination = true, bool extra = false,
            HttpClient httpClient = null)

            where TItemsRoot : BaseModelItemsRoot<TItem>, new()
            where TItem : BaseItem, new()
        {
            var respondData = await _requestProvider.GetAsyncPagination(url, httpClient, urlPagination, pagination, contentquery);

            if (respondData != null)
            {
                try
                {
                    var t = new TItemsRoot();
                    t.OnInitialize(respondData.Response, respondData.HttpResponseCode, respondData.ResponseHeaders, pagination, extra);
                    return t;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());

                }
            }

            return null;
        }


        public async Task<TItemRoot> Retrieve<TItemRoot, TItem>
            (string url,
            string pk = null,
            HttpClient httpClient = null,
            FormUrlEncodedContent contentquery = null)

            where TItemRoot : BaseModelItemRoot<TItem>, new()
            where TItem : BaseItem, new()
        {
            var retrieveUrl = url;
            if (!string.IsNullOrWhiteSpace(pk)
                &&
                !string.IsNullOrWhiteSpace(url))
            {
                retrieveUrl = url + pk + "/";
            }


            var respondData = await _requestProvider.GetAsync(retrieveUrl, httpClient, contentquery);

            if (respondData != null)
            {
                try
                {
                    var t = new TItemRoot();
                    t.OnInitializeItem(respondData.Response, respondData.HttpResponseCode, respondData.ResponseHeaders);
                    return t;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());

                }
            }

            return null;
        }


        public async Task<RootItemBase> Delete
            (string url,
            string pk = null,
            HttpClient httpClient = null)
        {
            var deleteUrl = url;
            if (!string.IsNullOrWhiteSpace(pk)
                &&
                !string.IsNullOrWhiteSpace(url))
            {
                deleteUrl = url + pk + "/";
            }

            var respondData = await _requestProvider.DeleteAsync(deleteUrl, httpClient);

            if (respondData != null)
            {
                try
                {
                    return new RootItemBase(respondData.Response, respondData.HttpResponseCode, respondData.ResponseHeaders);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());

                }
            }

            return null;
        }


        public async Task<TItemRoot> CreateUpdate<TItemRoot, TItem>
            (string url,
            string pk = null,
            MultipartFormDataContent formData = null,
            HttpContent httpContent = null,
            HttpClient httpClient = null,
            bool partialUpdate = false)

            where TItemRoot : BaseModelItemRoot<TItem>, new()
            where TItem : BaseItem, new()
        {
            var apiUrl = url;
            if (!string.IsNullOrWhiteSpace(pk)
                &&
                !string.IsNullOrWhiteSpace(url))
            {
                apiUrl = url + pk + "/";
            }

            RequestResponseItem respondData;
            if (string.IsNullOrWhiteSpace(pk))
            {
                if (formData != null)
                {
                    respondData = await _requestProvider.PostMultipartFormAsync(apiUrl, formData, httpClient);
                }
                else
                {
                    respondData = await _requestProvider.PostAsync(apiUrl, httpContent, httpClient);
                }

            }
            else
            {
                respondData = await _requestProvider.PutAsync(apiUrl, formData, httpClient, partialUpdate);
            }


            if (respondData != null)
            {
                try
                {
                    var t = new TItemRoot();
                    t.OnInitializeItem(respondData.Response, respondData.HttpResponseCode, respondData.ResponseHeaders);
                    return t;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());

                }
            }

            return null;
        }
        #endregion
    }
}
