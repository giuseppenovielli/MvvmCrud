using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Resources;
using System.Security.Cryptography.X509Certificates;
using MVVMCrud.Models.ItemRoot;
using MVVMCrud.Utils;
using MVVMCrud.Utils.Request;
using Newtonsoft.Json;

namespace MVVMCrud.Example.Utils.MVVMCrud
{
    public class CustomMVVMCrud : MVVMCrudApplication
    {
        public CustomMVVMCrud()
        {
        }

        public override BaseRequestSetupResponse SetupBaseRequestSetupResponse() => new CustomBaseRequestSetupResponse();

        public override ResourceManager SetupAppResourceManager()
        {
            return AppResources.ResourceManager;
        }

        public override HttpClient SetupHttpClient()
        {
            HttpClientHandler handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = CheckSSLConnection
            };

            var client = new HttpClient(handler);

            var authHeader = client.DefaultRequestHeaders;
            authHeader.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

    private bool CheckSSLConnection(HttpRequestMessage arg1, X509Certificate2 arg2, X509Chain arg3, SslPolicyErrors arg4)
    {
        return true;
    }

    public override void SetupRootItemBase(RootItemBase rootItemBase)
        {
            var httpCode = rootItemBase.HttpResponseCode;

            if (
                    httpCode != System.Net.HttpStatusCode.OK
                    &&
                    httpCode != System.Net.HttpStatusCode.NoContent
                    &&
                    httpCode != System.Net.HttpStatusCode.Created
                )
            {
                rootItemBase.IsError = true;
            }

        }

        public override void SetupPaginationRequest(List<KeyValuePair<string, string>> dataGet, bool pagination, int paginationSize)
        {
            if (pagination)
            {
                dataGet.Add(new KeyValuePair<string, string>("_page", "1"));
                if (paginationSize > 0)
                {
                    dataGet.Add(new KeyValuePair<string, string>("page_size", paginationSize.ToString()));
                }

            }
            else
            {
                dataGet.Add(new KeyValuePair<string, string>("page_size", "0"));

            }
        }

        public override void SetupPaginationItem(string item, HttpResponseHeaders responseHeader, PaginationItem paginationItem)
        {
            var link = responseHeader.GetValues("Link").FirstOrDefault();
            var pagination = LinkHeader.LinksFromHeader(link);
            if (pagination != null)
            {
                var nextLink = pagination.NextLink;
                paginationItem.NextUrl = nextLink.Replace("http://", "https://");

            }
        }

    }
}
