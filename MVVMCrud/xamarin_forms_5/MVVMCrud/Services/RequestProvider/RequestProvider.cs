using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MVVMCrud.Models.RequestResponse;

namespace MVVMCrud.Services.RequestProvider
{
    public class RequestProvider : IRequestProvider
    {
        public RequestProvider()
        {
        }

        public async Task<RequestResponseItem> GetAsync(string apiUrl, HttpClient httpClient = null, FormUrlEncodedContent content_query = null)
        {
            System.Diagnostics.Debug.WriteLine("\nMVVMCrud GetAsync");
            System.Diagnostics.Debug.WriteLine("GetAsync apiUrl = " + apiUrl);

            RequestResponseItem responseItem = null;

            if (!string.IsNullOrWhiteSpace(apiUrl))
            {
                var client = RequestProvider.CreateHttpClient(httpClient);

                try
                {
                    if (content_query != null)
                    {
                        apiUrl += "?" + await content_query.ReadAsStringAsync();
                    }

                    var response = await client.GetAsync(apiUrl);

                    string json = await response.Content.ReadAsStringAsync();
                    var responseStatus = response.StatusCode;

                    responseItem = new RequestResponseItem(responseStatus, json, response.Headers);

                    System.Diagnostics.Debug.WriteLine("ResposeCode = "+ responseStatus);
                    System.Diagnostics.Debug.WriteLine("message = " + json);
                }
                catch (Exception e)
                {


                    System.Diagnostics.Debug.WriteLine("GetAsync Exception = " + e.ToString());
                    System.Diagnostics.Debug.WriteLine("ResposeCode = null");
                    System.Diagnostics.Debug.WriteLine("message = " + e.ToString());
                }
            }
            else
            {
                throw new HttpRequestUrlEmptyExceptionEx();
            }

            return responseItem;
        }

        public async Task<RequestResponseItem> GetAsyncPagination(string apiUrl,
                                                                HttpClient httpClient = null,
                                                                string urlPagination = null,
                                                                bool pagination = true,
                                                                FormUrlEncodedContent content_query = null,
                                                                int paginationSize = 0,
                                                                string string_query = null,
                                                                Action<List<KeyValuePair<string, string>>, bool, int> paginationRequest = null)
        {
            System.Diagnostics.Debug.WriteLine("\nMVVMCrud GetAsyncPagination");
            System.Diagnostics.Debug.WriteLine("GetAsyncPagination apiUrl = " + apiUrl);

            RequestResponseItem responseItem = null;

            if (!string.IsNullOrWhiteSpace(apiUrl))
            {
                var client = RequestProvider.CreateHttpClient(httpClient);

                try
                {
                    if (string.IsNullOrWhiteSpace(urlPagination))
                    {
                        var dataGet = new List<KeyValuePair<string, string>>();

                        if (paginationRequest== null)
                        {
                            MVVMCrudApplication.Instance.SetupPaginationRequest(dataGet, pagination, paginationSize);
                        }
                        else
                        {
                            paginationRequest.Invoke(dataGet, pagination, paginationSize);
                        }

                        var content = new FormUrlEncodedContent(dataGet);

                        apiUrl += "?" + await content.ReadAsStringAsync();

                        if (content_query != null)
                        {
                            apiUrl += "&" + await content_query.ReadAsStringAsync();
                        }

                        if (!string.IsNullOrWhiteSpace(string_query))
                        {
                            apiUrl += "&" + string_query;
                        }
                    }
                    else
                    {
                        apiUrl = urlPagination;
                    }

                    System.Diagnostics.Debug.WriteLine("GetAsync apiUrl = " + apiUrl);

                    var response = await client.GetAsync(apiUrl);

                    string json = await response.Content.ReadAsStringAsync();
                    var responseStatus = response.StatusCode;

                    responseItem = new RequestResponseItem(responseStatus, json, response.Headers);

                    System.Diagnostics.Debug.WriteLine("ResposeCode = " + responseStatus);
                    System.Diagnostics.Debug.WriteLine("message = " + json);
                }
                catch (Exception e)
                {


                    System.Diagnostics.Debug.WriteLine("GetAsync Exception = " + e.ToString());
                    System.Diagnostics.Debug.WriteLine("ResposeCode = null");
                    System.Diagnostics.Debug.WriteLine("message = " + e.ToString());
                }
            }
            else
            {
                throw new HttpRequestUrlEmptyExceptionEx();
            }

            return responseItem;
        }

        public async Task<RequestResponseItem> PostAsync(string apiUrl, HttpContent httpContent = null, HttpClient httpClient = null) 
        {
            System.Diagnostics.Debug.WriteLine("\nMVVMCrud PostAsync");
            System.Diagnostics.Debug.WriteLine("PostAsync apiUrl = " + apiUrl);
            System.Diagnostics.Debug.WriteLine("PostAsync data = " + httpContent);

            RequestResponseItem responseItem = null;


            if (!string.IsNullOrWhiteSpace(apiUrl))
            {
                var client = RequestProvider.CreateHttpClient(httpClient);

                try
                {

                    var response = await client.PostAsync(apiUrl, httpContent);

                    string json = await response.Content.ReadAsStringAsync();
                    var responseStatus = response.StatusCode;

                    responseItem = new RequestResponseItem(responseStatus, json, response.Headers);


                    System.Diagnostics.Debug.WriteLine("httpcode = " + responseStatus);
                    System.Diagnostics.Debug.WriteLine("response = " + json);
                }
                catch (Exception e)
                {


                    System.Diagnostics.Debug.WriteLine("PostAsync Exception = " + e.ToString());
                    System.Diagnostics.Debug.WriteLine("httpcode = null");
                    System.Diagnostics.Debug.WriteLine("response = " + e.ToString());
                }
            }
            else
            {
                throw new HttpRequestUrlEmptyExceptionEx();
            }

            return responseItem;

        }

        public async Task<RequestResponseItem> PostMultipartFormAsync(string apiUrl, MultipartFormDataContent data = null, HttpClient httpClient = null)
        {
            System.Diagnostics.Debug.WriteLine("\nMVVMCrud PostMultipartFormAsync");
            System.Diagnostics.Debug.WriteLine("PostMultipartFormAsync apiUrl = " + apiUrl);
            System.Diagnostics.Debug.WriteLine("PostMultipartFormAsync data = " + data);


            RequestResponseItem responseItem = null;


            if (!string.IsNullOrWhiteSpace(apiUrl))
            {
                var client = RequestProvider.CreateHttpClient(httpClient);

                try
                {

                    var response = await client.PostAsync(apiUrl, data);

                    string json = await response.Content.ReadAsStringAsync();
                    var responseStatus = response.StatusCode;

                    responseItem = new RequestResponseItem(responseStatus, json, response.Headers);


                    System.Diagnostics.Debug.WriteLine("httpcode = " + responseStatus);
                    System.Diagnostics.Debug.WriteLine("response = " + json);
                }
                catch (Exception e)
                {


                    System.Diagnostics.Debug.WriteLine("PostAsync Exception = " + e.ToString());
                    System.Diagnostics.Debug.WriteLine("httpcode = null");
                    System.Diagnostics.Debug.WriteLine("response = " + e.ToString());
                }
            }
            else
            {
                throw new HttpRequestUrlEmptyExceptionEx();
            }

            return responseItem;
        }

        public async Task<RequestResponseItem> DeleteAsync(string apiUrl, HttpClient httpClient = null)
        {
            System.Diagnostics.Debug.WriteLine("\nMVVMCrud DeleteAsync");
            System.Diagnostics.Debug.WriteLine("DeleteAsync apiUrl = " + apiUrl);

            RequestResponseItem responseItem = null;


            if (!string.IsNullOrWhiteSpace(apiUrl))
            {
                var client = RequestProvider.CreateHttpClient(httpClient);

                try
                {

                    var response = await client.DeleteAsync(apiUrl);

                    string json = await response.Content.ReadAsStringAsync();
                    var responseStatus = response.StatusCode;

                    responseItem = new RequestResponseItem(responseStatus, json, response.Headers);


                    System.Diagnostics.Debug.WriteLine("httpcode = " + responseStatus);
                    System.Diagnostics.Debug.WriteLine("response = " + json);
                }
                catch (Exception e)
                {


                    System.Diagnostics.Debug.WriteLine("DeleteAsync Exception = " + e.ToString());
                    System.Diagnostics.Debug.WriteLine("httpcode = null");
                    System.Diagnostics.Debug.WriteLine("response = " + e.ToString());

                }
            }
            else
            {
                throw new HttpRequestUrlEmptyExceptionEx();
            }

            return responseItem;

        }

        public async Task<RequestResponseItem> PutAsync(string apiUrl, HttpContent data = null, HttpClient httpClient = null, bool partialUpdate = false)
        {
            System.Diagnostics.Debug.WriteLine("\nMVVMCrud PutAsync");
            System.Diagnostics.Debug.WriteLine("PutAsync apiUrl = " + apiUrl);
            System.Diagnostics.Debug.WriteLine("PutAsync partialUpdate = " + partialUpdate);

            if (data != null)
            {
                System.Diagnostics.Debug.WriteLine("PutAsync data = " + await data?.ReadAsStringAsync());
            }

            System.Diagnostics.Debug.WriteLine("PutAsync HttpClient = " + httpClient);


            RequestResponseItem responseItem = null;


            if (!string.IsNullOrWhiteSpace(apiUrl))
            {

                var client = RequestProvider.CreateHttpClient(httpClient);

                try
                {
                    HttpResponseMessage response = null;

                    if (!partialUpdate)
                    {
                        response = await client.PutAsync(apiUrl, data);
                    }
                    else
                    {
                        //https://stackoverflow.com/questions/47463000/c-sharp-xamarin-forms-add-custom-header-patch
                        var method = new HttpMethod("PATCH");
                        var request = new HttpRequestMessage(method, apiUrl)
                        {
                            Content = data
                        };
                        response = await client.SendAsync(request);
                    }

                    var json = await response.Content.ReadAsStringAsync();
                    var responseStatus = response.StatusCode;

                    responseItem = new RequestResponseItem(responseStatus, json, response.Headers);


                    System.Diagnostics.Debug.WriteLine("httpcode = " + responseStatus);
                    System.Diagnostics.Debug.WriteLine("response = " + json);
                }
                catch (Exception e)
                {


                    System.Diagnostics.Debug.WriteLine("PutAsync Exception = " + e.ToString());
                    System.Diagnostics.Debug.WriteLine("httpcode = null");
                    System.Diagnostics.Debug.WriteLine("response = " + e.ToString());
                }
            }
            else
            {
                throw new HttpRequestUrlEmptyExceptionEx();
            }

            return responseItem;

        }

        public async Task<RequestResponseItem> GetByteArray(string apiUrl, HttpClient httpClient = null, bool header = true)
        {
            System.Diagnostics.Debug.WriteLine("\nMVVMCrud GetByteArray");
            System.Diagnostics.Debug.WriteLine("GetByteArray apiUrl = " + apiUrl);

            RequestResponseItem responseItem = null;

            if (!string.IsNullOrWhiteSpace(apiUrl))
            {

                var client = RequestProvider.CreateHttpClient(httpClient, header: header);

                try
                {

                    var response = await client.GetAsync(apiUrl);

                    var byteArray = await response.Content.ReadAsByteArrayAsync();
                    var responseStatus = response.StatusCode;

                    responseItem = new RequestResponseItem(responseStatus, null, response.Headers, byteArray);

                    System.Diagnostics.Debug.WriteLine("ResposeCode = " + responseStatus);
                    System.Diagnostics.Debug.WriteLine("byteArray = " + byteArray);
                    System.Diagnostics.Debug.WriteLine("byteArray Lenght KB = " + byteArray.Length / 1024);
                }
                catch (Exception e)
                {


                    System.Diagnostics.Debug.WriteLine("GetByteArray Exception = " + e.ToString());
                    System.Diagnostics.Debug.WriteLine("ResposeCode = null");
                    System.Diagnostics.Debug.WriteLine("message = " + e.ToString());
                }
            }
            else
            {
                throw new HttpRequestUrlEmptyExceptionEx();
            }

            return responseItem;
        }

        static HttpClient CreateHttpClient(HttpClient httpClient = null, bool header = true)
        {
            var httpClientInstance = httpClient;
            if (header)
            {
                if (httpClientInstance == null)
                {
                    httpClientInstance = MVVMCrudApplication.Instance?.HttpClient;
                    if (httpClientInstance == null)
                    {
                        httpClientInstance = new HttpClient();
                    }
                }
            }
            else
            {
                httpClientInstance = new HttpClient();
            }

            return httpClientInstance;
        }

    }
}
