using System.Net;
using System.Net.Http.Headers;

namespace MVVMCrud.Models.RequestResponse
{
    public class RequestResponseItem
    {
        public HttpStatusCode HttpResponseCode { get; private set; }
        public string Response { get; private set; }
        public byte[] ByteArray { get; private set; }
        public HttpResponseHeaders ResponseHeaders { get; set; }

        public RequestResponseItem(HttpStatusCode httpStatus,
                                    string response,
                                    HttpResponseHeaders responseHeaders,
                                    byte[] byteArray = null)
        {

            HttpResponseCode = httpStatus;
            Response = response;
            ResponseHeaders = responseHeaders;
            ByteArray = byteArray;
        }
    }
}
