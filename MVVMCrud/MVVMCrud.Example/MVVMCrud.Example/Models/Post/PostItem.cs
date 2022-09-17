using MVVMCrud.Models.Base;
using Newtonsoft.Json;

namespace MVVMCrud.Example.Models.Post
{
    public class PostItem : BaseItem
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("userId")]
        public long UserId { get; set; }

        public PostItem()
        {
        }
    }
}
