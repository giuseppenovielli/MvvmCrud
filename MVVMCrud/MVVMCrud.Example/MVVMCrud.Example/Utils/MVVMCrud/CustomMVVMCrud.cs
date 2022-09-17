using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
            AppResourceManager = AppResources.ResourceManager;
        }

        public override BaseRequestSetupResponse SetupBaseRequestSetupResponse() => new CustomBaseRequestSetupResponse();

        public override HttpClient SetupHttpClient()
        {
            var client = base.SetupHttpClient();

            var authHeader = client.DefaultRequestHeaders;
            authHeader.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
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
