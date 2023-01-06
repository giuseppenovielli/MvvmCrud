using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MVVMCrud.Models.RequestResponse;

namespace MVVMCrud.Services.RequestProvider
{
    public interface IRequestProvider
    {
        Task<RequestResponseItem> PostAsync(string apiUrl, HttpContent data = null, HttpClient httpClient = null);
        Task<RequestResponseItem> PostMultipartFormAsync(string apiUrl, MultipartFormDataContent data = null, HttpClient httpClient = null);
        Task<RequestResponseItem> GetAsync(string apiUrl, HttpClient httpClient = null, FormUrlEncodedContent content_query = null);
        Task<RequestResponseItem> GetAsyncPagination(string apiUrl, HttpClient httpClient = null, string urlPagination = null, bool pagination = true, FormUrlEncodedContent content_query = null, int paginationSize = 0, string string_query = null, Action<List<KeyValuePair<string, string>>, bool, int> paginationRequest = null);
        Task<RequestResponseItem> PutAsync(string apiUrl, HttpContent data = null, HttpClient httpClient = null);
        Task<RequestResponseItem> DeleteAsync(string apiUrl, HttpClient httpClient = null);
        Task<RequestResponseItem> GetByteArray(string apiUrl, HttpClient httpClient = null, bool header = true);
    }
}
