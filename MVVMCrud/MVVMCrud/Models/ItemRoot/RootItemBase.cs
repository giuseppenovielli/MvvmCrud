using System.Net;
using System.Net.Http.Headers;

namespace MVVMCrud.Models.ItemRoot
{
    public class RootItemBase
    {
        public string ResponseText { get; private set; }
        public HttpStatusCode HttpResponseCode { get; private set; }
        public HttpResponseHeaders ResponseHeader { get; private set; }
        public bool IsError { get; set; }

        public RootItemBase(string item, HttpStatusCode httpStatus, HttpResponseHeaders responseHeader)
        {
            ResponseText = item;
            HttpResponseCode = httpStatus;
            ResponseHeader = responseHeader;

            MVVMCrudApplication.Instance?.SetupRootItemBase(this);
        }

        public RootItemBase()
        {
        }
    }
}
