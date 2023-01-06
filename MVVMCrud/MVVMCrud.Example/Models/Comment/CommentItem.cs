using MVVMCrud.Models.Base;
using Newtonsoft.Json;

namespace MVVMCrud.Example.Models.Comment
{
    public class CommentItem : BaseItem
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("postId")]
        public long PostId { get; set; }

        public CommentItem()
        {
        }
    }
}
