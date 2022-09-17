using System;
using System.Threading.Tasks;
using MVVMCrud.Models.Base;
using MVVMCrud.Models.ItemRoot;
using MVVMCrud.Models.Request;

namespace MVVMCrud.Utils.Request
{
    public class BaseRequest<TBaseModelRoot>
        where TBaseModelRoot : BaseModelRoot
    {
        public RequestConfig RequestConfig { get; set; }
        public TBaseModelRoot BaseModelRoot { get; set; }

        public BaseRequest(RequestConfig requestConfig = null)
        {
            RequestConfig = requestConfig;

            if (RequestConfig == null)
            {
                RequestConfig = GetRequestConfig();
            }
        }



        public async virtual Task<TBaseModelRoot> SetupSendRequest()
        {
            BaseModelRoot = null;

            if (Utils.IsConnected())
            {
                RequestConfig.ItemSetup?.Invoke();

                RequestConfig.SetupShowLoadingPopup?.Invoke();

                BaseModelRoot = await SetupRequest();

                if (BaseModelRoot != null)
                {
                    return SetupResponse(BaseModelRoot);
                }
                else
                {
                    SetupResponseNull();
                }

                RequestConfig.SetupHideLoadingPopup?.Invoke();
            }
            else
            {
                SetupNoConnection();
            }

            return null;
        }

        public virtual TBaseModelRoot SetupResponse(TBaseModelRoot baseModelRoot)
        {
            var rootItemBase = baseModelRoot.RootItemBase;
            if (rootItemBase != null)
            {
                if (RequestConfig.HttpStatusCodeList.Contains(rootItemBase.HttpResponseCode)
                    ||
                    RequestConfig.GetItemIfError)
                {
                    return baseModelRoot;
                }
                else
                {
                    SetupResponseError(rootItemBase);
                }
            }

            return null;
        }

        public virtual void SetupResponseError(RootItemBase rootItemBase)
        {
            throw new NotImplementedException();
        }

        public virtual void SetupResponseNull()
        {
            SetupDisplayMessage(SetupDisplayMessageTitle(), SetupResponseNullMessageText());
        }

        public virtual string SetupResponseNullMessageText()
        {
            return MVVMCrudApplication.GetNoConnectionText();
        }


        

        public async virtual Task<TBaseModelRoot> SetupRequest() => await Task.FromResult<TBaseModelRoot>(null);

        public virtual void SetupNoConnection()
        {
            SetupDisplayMessage(SetupDisplayMessageTitle(), SetupNoConnectionText());
        }

        public virtual string SetupNoConnectionText()
        {
            return MVVMCrudApplication.GetNoConnectionText();
        }

        public virtual void SetupDisplayMessage(string title, string message, string ok = null)
        {
            Utils.DisplaySimplyAlert(title, message, ok);
        }

        public virtual string SetupDisplayMessageTitle()
        {
            return RequestConfig.TitlePage;
        }

        public static RequestConfig GetRequestConfig()
        {
            var requestConfig = MVVMCrudApplication.Instance?.RequestConfig;
            if (requestConfig == null)
            {
                requestConfig = new RequestConfig();
            }
            return requestConfig;
        }
    }
}
