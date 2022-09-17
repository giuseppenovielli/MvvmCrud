using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MVVMCrud.Models.Base
{
    public class BaseModelItemRoot<M> : BaseModelRoot
        where M : BaseItem, new()
    {
        public M Item { get; set; }

        public BaseModelItemRoot(){}

        public BaseModelItemRoot(string item, HttpStatusCode httpStatus, HttpResponseHeaders responseHeader, bool customDataExtract = false, object data = null) : base(item, httpStatus, responseHeader, data, customDataExtract)
        {
        }

        protected override void OnDataObject(JObject jObject)
        {
            base.OnDataObject(jObject);

            if (CustomDataExtract)
            {
                Item = new M();
                Item.InitializeJObject(jObject);

            }
            else
            {
                Item = JsonConvert.DeserializeObject<M>(jObject.ToString());
                Item.InitializeIDLong();
            }
        }
    }
}
