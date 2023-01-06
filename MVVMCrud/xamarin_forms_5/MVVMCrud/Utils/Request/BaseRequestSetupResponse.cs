using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MVVMCrud.Models.Base;
using MVVMCrud.Models.ItemRoot;
using Xamarin.Forms;

namespace MVVMCrud.Utils.Request
{
    public class BaseRequestSetupResponse
    {
        private BaseModelRoot _baseModelRoot;
        private string _title;
        private List<HttpStatusCode> _httpStatusCode;
        private bool _getItemIfError;

        public BaseRequestSetupResponse()
        {
        }

        public virtual void Initialize(string title = null)
        {
            _title = title;
            if (_title == null)
            {
                _title = string.Empty;
            }
        }

        public virtual bool Analize(BaseModelRoot baseModelRoot, List<HttpStatusCode> httpStatusCode = null, bool getItemIfError = false)
        {
            _baseModelRoot = baseModelRoot;
            _httpStatusCode = httpStatusCode;
            _getItemIfError = getItemIfError;

            if (_httpStatusCode == null)
            {
                _httpStatusCode = new List<HttpStatusCode>() { HttpStatusCode.OK };
            }

            if (_baseModelRoot != null)
            {
                var rootItemBase = _baseModelRoot.RootItemBase;
                if (rootItemBase != null)
                {
                    if (_httpStatusCode.Contains(rootItemBase.HttpResponseCode)
                        ||
                        _getItemIfError)
                    {
                        return true;
                    }
                    else
                    {
                        SetupResponseError(rootItemBase);
                    }
                }
                else
                {
                    SetupResponseNull(_baseModelRoot);
                }
            }
            else
            {
                SetupResponseNull(_baseModelRoot);
            }

            return false;
        }

        public virtual void SetupResponseError(RootItemBase rootItemBase)
        {
            if (rootItemBase != null)
            {
                var httpResponseCode = rootItemBase.HttpResponseCode;

                switch (httpResponseCode)
                {
                    case HttpStatusCode.BadRequest:
                        SetupBadRequest(rootItemBase, _title);
                        break;

                    case HttpStatusCode.Unauthorized:
                        SetupUnauthorized(rootItemBase, _title);
                        break;

                    case HttpStatusCode.Found:
                        SetupRedirect(rootItemBase, _title);
                        break;

                    case HttpStatusCode.NotFound:
                        SetupNotFound(rootItemBase, _title);
                        break;

                    default:
                        SetupGenericError(rootItemBase, _title);
                        break;

                }
            }
        }

        public virtual void SetupRedirect(RootItemBase rootItemBase, string title = null)
        {
        }

        public virtual void SetupBadRequest(RootItemBase rootItemBase, string titlePage = null)
        {
            
        }

        public virtual void SetupUnauthorized(RootItemBase rootItemBase, string titlePage = null)
        {
            
        }

        public virtual void SetupNotFound(RootItemBase rootItemBase, string titlePage = null)
        {
            
        }

        public virtual void SetupGenericError(RootItemBase rootItemBase, string titlePage = null)
        {
            
        }

        public virtual void SetupResponseNull(BaseModelRoot baseModelRoot, string titlePage = null)
        {
            SetupDisplayMessage(titlePage, SetupResponseNullMessage(titlePage));
        }

        public virtual string SetupDisplayMessageTitle()
        {
            return _title;
        }

        public virtual string SetupResponseNullMessage(string title)
        {
            return MVVMCrudApplication.GetGenericErrorText();
        }

        public virtual void SetupDisplayMessage(string title, string message, string ok = null)
        {
            if (!string.IsNullOrWhiteSpace(message)
                &&
                !string.IsNullOrWhiteSpace(ok))
            {
                Application.Current.MainPage.DisplayAlert(title, message, ok);
            }
            
        }

        public virtual void SetupResponseNoConnection(string title)
        {
            SetupDisplayMessage(title, SetupResponseNoConnectionMessage());
        }

        public virtual string SetupResponseNoConnectionMessage()
        {
            return MVVMCrudApplication.GetNoConnectionText();
        }

        public virtual Task SetupShowLoading()
        {
            return Task.CompletedTask;
        }

        public virtual Task SetupHideLoading()
        {
            return Task.CompletedTask;
        }
    }
}
