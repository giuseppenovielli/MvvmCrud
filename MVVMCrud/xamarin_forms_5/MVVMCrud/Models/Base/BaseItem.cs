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
        public bool ShouldSerializeIdLong() => false;

        [JsonProperty("can_edit")]
        public bool CanEdit { get; set; }
        public bool ShouldSerializeCanEdit() => false;

        [JsonProperty("can_delete")]
        public bool CanDelete { get; set; }
        public bool ShouldSerializeCanDelete() => false;

        public BaseItem()
        {
        }

        public BaseItem(JObject item)
        {
            try
            {
                InitializeJObject(item);

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
                InitializeIDLong();

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
            Id = string.Empty;
            if (item.ContainsKey("id"))
            {
                Id = item.GetValue("id").ToString();
            }

        }


    }
}
