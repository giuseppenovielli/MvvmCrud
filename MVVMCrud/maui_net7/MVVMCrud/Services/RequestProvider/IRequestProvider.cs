using System.Net.Http;
using System.Threading.Tasks;
using HRCoffee.Models.RequestResponse;

namespace HRCoffee.Services.RequestProvider
{
    public interface IRequestProvider
    {

        Task<RequestResponseItem> PostAsync(string apiUrl, HttpContent data = null, string token = null, HttpClient httpClient = null);
        Task<RequestResponseItem> PostMultipartFormAsync(string apiUrl, MultipartFormDataContent data = null, string token = null, HttpClient httpClient = null);
        Task<RequestResponseItem> GetAsync(string apiUrl, string token = null, HttpClient httpClient = null, FormUrlEncodedContent content_query = null);
        Task<RequestResponseItem> GetAsyncPagination(string apiUrl, string token = null, HttpClient httpClient = null, string urlPagination = null, bool pagination = true, FormUrlEncodedContent content_query = null, int paginationSize = 0, string string_query = null);
        Task<RequestResponseItem> PutAsync(string apiUrl, HttpContent data = null, string token = null, HttpClient httpClient = null);
        Task<RequestResponseItem> DeleteAsync(string apiUrl, string token = null, HttpClient httpClient = null);
        Task<RequestResponseItem> GetByteArray(string apiUrl, HttpClient httpClient = null, bool header = true);
    }
}
