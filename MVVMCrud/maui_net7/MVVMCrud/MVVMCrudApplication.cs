using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Resources;
using MVVMCrud.CustomView;
using MVVMCrud.Models.ItemRoot;
using MVVMCrud.Services.Request;
using MVVMCrud.Services.RequestProvider;
using MVVMCrud.Utils;
using MVVMCrud.Utils.Request;
using MVVMCrud.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Prism.Ioc;

namespace MVVMCrud
{
    public class MVVMCrudApplication
    {
        public const string PAGINATION_RESULT_NAME = "results";
        public const string RESULT_LIST = "results";

        #region EmptyView
        public ContentView EmptyView { get; set; }
        #endregion

        #region LoadingMoreView
        public ContentView LoadingMoreView { get; set; }
        #endregion

        #region AppResource Host App
        public ResourceManager AppResourceManager { get; set; }
        #endregion

        #region Message resources
        public string ConfirmDeleteText { get; set; }
        public string ConfirmUploadText { get; set; }
        public string EmptyText { get; set; }
        public string RefreshText { get; set; }
        public string LoadingMoreText { get; set; }
        public string LoadingText { get; set; }
        public string OKText { get; set; }
        public string CancelText { get; set; }
        public string SearchPlaceholder { get; set; }
        public string NotFound { get; set; }
        public string NoConnectionText { get; set; }
        public string GenericErrorText { get; set; }

        public string YesText { get; set; }
        public string NoText { get; set; }
        public string NewItemUploadText { get; set; }
        public string EditItemUploadText { get; set; }
        public string DeleteItemUploadText { get; set; }
        #endregion

        public static MVVMCrudApplication Instance { get; set; }

        public HttpClient HttpClient { get; set; }

        public List<string> IdMessagingCenterActiveList { get; set; }

        public MVVMCrudApplication()
        {
            Instance = this;

            IdMessagingCenterActiveList = new List<string>();

            HttpClient = SetupHttpClient();
        }

        public virtual HttpClient SetupHttpClient() => new HttpClient();

        public static string GetConfirmDeleteText() => Instance?.ConfirmDeleteText != null ? Instance.ConfirmDeleteText : AppResources.msg_confirm_delete;
        public static string GetConfirmUploadText() => Instance?.ConfirmUploadText != null ? Instance.ConfirmUploadText : AppResources.msg_confirm_upload;
        public static string GetEmptyText() => Instance?.EmptyText != null ? Instance.EmptyText : AppResources.msg_empty_list;
        public static string GetRefreshText() => Instance?.RefreshText != null ? Instance.RefreshText : AppResources.action_refresh;
        public static string GetLoadingMoreText() => Instance?.LoadingMoreText != null ? Instance.LoadingMoreText : AppResources.list_loading_more;
        public static string GetLoadingText() => Instance?.LoadingMoreText != null ? Instance.LoadingText : AppResources.msg_loading;
        public static string GetOKText() => Instance?.OKText != null ? Instance.OKText : AppResources.ok;
        public static string GetCancelText() => Instance?.CancelText != null ? Instance.CancelText : AppResources.cancel;
        public static string GetSearchPlaceholder() => Instance?.SearchPlaceholder != null ? Instance.SearchPlaceholder : AppResources.placeholder_search;
        public static string GetNotFound() => Instance?.NotFound != null ? Instance.NotFound : AppResources.msg_search_not_result_found;
        public static string GetNoConnectionText() => Instance?.NoConnectionText != null ? Instance.NoConnectionText : AppResources.error_internet_connection;
        public static string GetGenericErrorText() => Instance?.GenericErrorText != null ? Instance.GenericErrorText : AppResources.error_generic;

        public static string GetYesText() => Instance?.YesText != null ? Instance.YesText : AppResources.action_yes;
        public static string GetNoText() => Instance?.NoText != null ? Instance.NoText : AppResources.action_no;
        public static string GetNewItemUploadText() => Instance?.NewItemUploadText != null ? Instance.NewItemUploadText : AppResources.item_added_successfully;
        public static string GetEditItemUploadText() => Instance?.EditItemUploadText != null ? Instance.EditItemUploadText : AppResources.item_update_successfully;
        public static string GetDeleteItemUploadText() => Instance?.DeleteItemUploadText != null ? Instance.DeleteItemUploadText : AppResources.item_deleted_successfully;


        public static ContentView GetEmptyView() => Instance?.EmptyView != null ? Instance.EmptyView : new EmptyView();
        public static ContentView GetLoadingMoreView() => Instance?.LoadingMoreView != null ? Instance.LoadingMoreView : new LoadingMoreView();
        public static ResourceManager GetAppResourceManager() => Instance?.AppResourceManager != null ? Instance.AppResourceManager : AppResources.ResourceManager;

        public virtual BaseRequestSetupResponse SetupBaseRequestSetupResponse() => new BaseRequestSetupResponse();


        public virtual List<JsonConverter> GetJsonConverters()
        {
            var l = new List<JsonConverter>
            {
            };

            return l;

        }
        public virtual void SetupPaginationItem(string item, HttpResponseHeaders responseHeader, PaginationItem paginationItem){}

        public virtual void SetupRootItemBase(RootItemBase rootItemBase){}

        public virtual void SetupPaginationRequest(RootItemBase rootItemBase) { }

        public virtual void SetupPaginationRequest(List<KeyValuePair<string, string>> dataGet,
                                                    bool pagination,
                                                    int paginationSize)
        { }

        public virtual JsonSerializerSettings SetupJsonSettingsSerialize(bool ignoreJsonProperty = true)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            };

            if (ignoreJsonProperty)
            {
                settings.ContractResolver = new IgnoreJsonPropertyContractResolver();
            }

            settings.Converters = GetJsonConverters();

            return settings;
        }


        public virtual JsonSerializerSettings SetupJsonSettingsDeserialize(bool ignoreJsonProperty=false)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new IgnoreJsonPropertyContractResolver()
            };

            if (ignoreJsonProperty)
            {
                settings.ContractResolver = new IgnoreJsonPropertyContractResolver();
            }

            settings.Converters = GetJsonConverters();

            return settings;
        }

        public virtual JsonSerializerSettings SetupJsonUploadSettingsSerialize()
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            };
           
            return settings;
        }

        public static void RegisterServices(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MVVMCrudModalNavigationPage>();

            containerRegistry.RegisterSingleton<IRequestProvider, RequestProvider>();
            containerRegistry.RegisterSingleton<IRequestService, RequestService>();
        }

        public static string GetLastPageUUID()
        {
            var uuid = string.Empty;
            if (Instance != null)
            {
                var uuidInstance = Instance.IdMessagingCenterActiveList.LastOrDefault();
                if (!string.IsNullOrWhiteSpace(uuidInstance))
                {
                    uuid = uuidInstance;
                }
            }
            return uuid;
        }
    }

}
