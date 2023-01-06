using System.Net.Http.Headers;

namespace MVVMCrud.Models.ItemRoot
{
    public class PaginationItem
    {
        public int Count { get; set; }
        public string NextUrl { get; set; }
        public string PreviuosUrl { get; set; }

        public PaginationItem(string item, HttpResponseHeaders responseHeader)
        {
            MVVMCrudApplication.Instance.SetupPaginationItem(item, responseHeader, this);
        }
    }
}
