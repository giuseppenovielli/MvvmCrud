using System;
using System.Collections.Generic;
using System.Net;

namespace MVVMCrud.Models.Request
{
    public class RequestConfig
    {
        public string TitlePage { get; set; }
        public Action ItemSetup { get; set; }
        public bool GetItemIfError { get; set; }
        public Action SetupShowLoadingPopup { get; set; }
        public Action SetupHideLoadingPopup { get; set; }
        public List<HttpStatusCode> HttpStatusCodeList { get; set; }

        public RequestConfig()
        {
            if (TitlePage == null)
            {
                TitlePage = string.Empty;
            }

            if (HttpStatusCodeList == null)
            {
                HttpStatusCodeList = new List<HttpStatusCode>()
                {
                    HttpStatusCode.OK
                };
            }
        }
    }
}
