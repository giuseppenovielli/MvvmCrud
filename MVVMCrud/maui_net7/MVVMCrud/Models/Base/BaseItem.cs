using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Mvvm;

namespace MVVMCrud.Models.Base
{
    public class BaseItem : BindableBase
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public long IdLong { get; set; }

        public BaseItem()
        {
        }

        public BaseItem(JObject item)
        {
            try
            {
                InitializeJObject(item);
                InitializeIDLong();

            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        public virtual void InitializeJObject(JObject item)
        {
            try
            {
                InitializeJObjectTryCatch(item);

            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        public virtual void InitializeIDLong()
        {
            try
            {
                IdLong = Convert.ToInt64(Id);
            }
            catch (System.Exception)
            {
            }
        }

        public virtual void InitializeJObjectTryCatch(JObject item)
        {
            Id = item.GetValue("id").ToString();
        }
    }
}
