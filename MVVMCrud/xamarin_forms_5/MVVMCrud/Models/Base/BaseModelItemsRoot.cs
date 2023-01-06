using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MVVMCrud.Models.Base
{
    public class BaseModelItemsRoot<M> : BaseModelRoot
        where M : BaseItem, new()
    {
        public List<M> Items { get; set; }

        public BaseModelItemsRoot(){}

        public BaseModelItemsRoot(string response, HttpStatusCode httpStatus, HttpResponseHeaders responseHeader, bool pagination = true, bool extra = false, bool customDataExtract = false, object data = null) : base(response, httpStatus, responseHeader, pagination, extra, customDataExtract, data)
        {
        }

        protected override void OnDataArray(JArray jArray)
        {
            base.OnDataArray(jArray);

            Items = new List<M>();

            foreach (var item in jArray)
            {
                var itemJObject = item.ToObject<JObject>();

                if (CustomDataExtract)
                {
                    var itemModel = new M();
                    itemModel.InitializeJObject(itemJObject);
                    Items.Add(itemModel);
                }
                else
                {
                    var jsonSettings = MVVMCrudApplication.Instance.SetupJsonSettingsDeserialize();
                    var jsonNew = JsonConvert.DeserializeObject<M>(itemJObject.ToString(), jsonSettings);
                    jsonNew.InitializeIDLong();
                    Items.Add(jsonNew);
                }
            }
        }
    }
}
