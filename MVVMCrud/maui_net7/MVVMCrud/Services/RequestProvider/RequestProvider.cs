using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HRCoffee.Models.RequestResponse;
using HRCoffee.Services.Platform;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace HRCoffee.Services.RequestProvider
{
    public class RequestProvider : IRequestProvider
    {
        private string _clientId;
        private string _clientVersione;
        private readonly MediaTypeWithQualityHeaderValue _mediaTypeWithQualityHeaderValue;

        public RequestProvider(IPlatformService platformService)
        {
            _clientId = "android";
            if (Device.RuntimePlatform == Device.iOS)
            {
                _clientId = "ios";
            }


            _clientVersione = platformService.GetAppVersion();
            if (string.IsNullOrWhiteSpace(_clientVersione))
            {
                _clientVersione = string.Empty;
            }

            _mediaTypeWithQualityHeaderValue = new MediaTypeWithQualityHeaderValue("application/json");
        }

        public async Task<RequestResponseItem> GetAsync(string apiUrl, string token = null, HttpClient httpClient = null, FormUrlEncodedContent content_query = null)
        {
            System.Diagnostics.Debug.WriteLine("\nGetAsync");
            System.Diagnostics.Debug.WriteLine("GetAsync apiUrl = " + apiUrl);
            System.Diagnostics.Debug.WriteLine("GetAsync token = " + token);
            System.Diagnostics.Debug.WriteLine("GetAsync HttpClient = " + httpClient);

            RequestResponseItem responseItem = null;

            if (!string.IsNullOrWhiteSpace(apiUrl))
            {
                var client = CreateHttpClient(httpClient, token);

                try
                {

                    if (content_query != null)
                    {
                        apiUrl += "?" + await content_query.ReadAsStringAsync();
                    }

                    var response = await client.GetAsync(apiUrl);

                    string json = await response.Content.ReadAsStringAsync();
                    var responseStatus = response.StatusCode;

                    responseItem = new RequestResponseItem(responseStatus, json);

                    System.Diagnostics.Debug.WriteLine("ResposeCode = " + responseStatus);
                    System.Diagnostics.Debug.WriteLine("message = " + json);
                }
                catch (Exception e)
                {


                    System.Diagnostics.Debug.WriteLine("GetAsync Exception = " + e.ToString());
                    System.Diagnostics.Debug.WriteLine("ResposeCode = null");
                    System.Diagnostics.Debug.WriteLine("message = " + e.ToString());
                    var properties = new Dictionary<string, string>
                    {
                        { "apiUrl", apiUrl },
                    };
                    Crashes.TrackError(e, properties);
                }
            }
            else
            {
                throw new HttpRequestUrlEmptyExceptionEx();
            }

            return responseItem;
        }

        public async Task<RequestResponseItem> GetAsyncPagination(string apiUrl, string token = null, HttpClient httpClient = null, string urlPagination = null, bool pagination = true, FormUrlEncodedContent content_query = null, int paginationSize = 0, string string_query = null)
        {
            System.Diagnostics.Debug.WriteLine("\nGetAsync");
            System.Diagnostics.Debug.WriteLine("GetAsync token = " + token);
            System.Diagnostics.Debug.WriteLine("GetAsync HttpClient = " + httpClient);

            RequestResponseItem responseItem = null;

            if (!string.IsNullOrWhiteSpace(apiUrl))
            {
                var client = CreateHttpClient(httpClient, token);

                try
                {
                    if (string.IsNullOrWhiteSpace(urlPagination))
                    {
                        var dataGet = new List<KeyValuePair<string, string>>();

                        if (pagination)
                        {
                            dataGet.Add(new KeyValuePair<string, string>("page", "1"));
                            if (paginationSize > 0)
                            {
                                dataGet.Add(new KeyValuePair<string, string>("page_size", paginationSize.ToString()));
                            }

                        }
                        else
                        {
                            dataGet.Add(new KeyValuePair<string, string>("page_size", "0"));

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

                    responseItem = new RequestResponseItem(responseStatus, json);

                    System.Diagnostics.Debug.WriteLine("ResposeCode = " + responseStatus);
                    System.Diagnostics.Debug.WriteLine("message = " + json);
                }
                catch (Exception e)
                {


                    System.Diagnostics.Debug.WriteLine("GetAsync Exception = " + e.ToString());
                    System.Diagnostics.Debug.WriteLine("ResposeCode = null");
                    System.Diagnostics.Debug.WriteLine("message = " + e.ToString());
                    var properties = new Dictionary<string, string>
                    {
                        { "apiUrl", apiUrl },
                    };
                    Crashes.TrackError(e, properties);
                }
            }
            else
            {
                throw new HttpRequestUrlEmptyExceptionEx();
            }

            return responseItem;
        }


        public async Task<RequestResponseItem> PostAsync(string apiUrl, HttpContent data = null, string token = null, HttpClient httpClient = null)
        {


            System.Diagnostics.Debug.WriteLine("\nPostAsync");
            System.Diagnostics.Debug.WriteLine("PostAsync apiUrl = " + apiUrl);
            System.Diagnostics.Debug.WriteLine("PostAsync data = " + data);
            System.Diagnostics.Debug.WriteLine("PostAsync token = " + token);
            System.Diagnostics.Debug.WriteLine("PostAsync HttpClient = " + httpClient);


            RequestResponseItem responseItem = null;


            if (!string.IsNullOrWhiteSpace(apiUrl))
            {
                var client = CreateHttpClient(httpClient, token);

                try
                {

                    //var content = new StringContent(JsonConvert.SerializeObject(data));
                    var response = await client.PostAsync(apiUrl, data);

                    string json = await response.Content.ReadAsStringAsync();
                    var responseStatus = response.StatusCode;

                    responseItem = new RequestResponseItem(responseStatus, json);


                    System.Diagnostics.Debug.WriteLine("httpcode = " + responseStatus);
                    System.Diagnostics.Debug.WriteLine("response = " + json);
                }
                catch (Exception e)
                {


                    System.Diagnostics.Debug.WriteLine("PostAsync Exception = " + e.ToString());
                    System.Diagnostics.Debug.WriteLine("httpcode = null");
                    System.Diagnostics.Debug.WriteLine("response = " + e.ToString());
                    var properties = new Dictionary<string, string>
                    {
                        { "apiUrl", apiUrl },
                    };
                    Crashes.TrackError(e, properties);
                }
            }
            else
            {
                throw new HttpRequestUrlEmptyExceptionEx();
            }

            return responseItem;

        }

        public async Task<RequestResponseItem> PostMultipartFormAsync(string apiUrl, MultipartFormDataContent data = null, string token = null, HttpClient httpClient = null)
        {
            System.Diagnostics.Debug.WriteLine("\nPostMultipartFormAsync");
            System.Diagnostics.Debug.WriteLine("PostMultipartFormAsync apiUrl = " + apiUrl);
            System.Diagnostics.Debug.WriteLine("PostMultipartFormAsync data = " + data);
            System.Diagnostics.Debug.WriteLine("PostMultipartFormAsync token = " + token);
            System.Diagnostics.Debug.WriteLine("PostMultipartFormAsync HttpClient = " + httpClient);


            RequestResponseItem responseItem = null;


            if (!string.IsNullOrWhiteSpace(apiUrl))
            {
                var client = CreateHttpClient(httpClient, token);

                try
                {

                    var response = await client.PostAsync(apiUrl, data);

                    string json = await response.Content.ReadAsStringAsync();
                    var responseStatus = response.StatusCode;

                    responseItem = new RequestResponseItem(responseStatus, json);


                    System.Diagnostics.Debug.WriteLine("httpcode = " + responseStatus);
                    System.Diagnostics.Debug.WriteLine("response = " + json);
                }
                catch (Exception e)
                {


                    System.Diagnostics.Debug.WriteLine("PostAsync Exception = " + e.ToString());
                    System.Diagnostics.Debug.WriteLine("httpcode = null");
                    System.Diagnostics.Debug.WriteLine("response = " + e.ToString());

                    var properties = new Dictionary<string, string>
                    {
                        { "apiUrl", apiUrl },
                    };
                    Crashes.TrackError(e, properties);
                }
            }
            else
            {
                throw new HttpRequestUrlEmptyExceptionEx();
            }

            return responseItem;
        }

        public async Task<RequestResponseItem> DeleteAsync(string apiUrl, string token = null, HttpClient httpClient = null)
        {


            System.Diagnostics.Debug.WriteLine("\nDeleteAsync");
            System.Diagnostics.Debug.WriteLine("DeleteAsync apiUrl = " + apiUrl);
            System.Diagnostics.Debug.WriteLine("DeleteAsync token = " + token);
            System.Diagnostics.Debug.WriteLine("DeleteAsync HttpClient = " + httpClient);


            RequestResponseItem responseItem = null;


            if (!string.IsNullOrWhiteSpace(apiUrl))
            {
                var client = CreateHttpClient(httpClient, token);

                try
                {

                    var response = await client.DeleteAsync(apiUrl);

                    string json = await response.Content.ReadAsStringAsync();
                    var responseStatus = response.StatusCode;

                    responseItem = new RequestResponseItem(responseStatus, json);


                    System.Diagnostics.Debug.WriteLine("httpcode = " + responseStatus);
                    System.Diagnostics.Debug.WriteLine("response = " + json);
                }
                catch (Exception e)
                {


                    System.Diagnostics.Debug.WriteLine("DeleteAsync Exception = " + e.ToString());
                    System.Diagnostics.Debug.WriteLine("httpcode = null");
                    System.Diagnostics.Debug.WriteLine("response = " + e.ToString());
                    var properties = new Dictionary<string, string>
                    {
                        { "apiUrl", apiUrl },
                    };
                    Crashes.TrackError(e, properties);
                }
            }
            else
            {
                throw new HttpRequestUrlEmptyExceptionEx();
            }

            return responseItem;

        }


        public async Task<RequestResponseItem> PutAsync(string apiUrl, HttpContent data = null, string token = null, HttpClient httpClient = null)
        {


            System.Diagnostics.Debug.WriteLine("\nPutAsync");
            System.Diagnostics.Debug.WriteLine("PutAsync apiUrl = " + apiUrl);

            if (data != null)
            {
                System.Diagnostics.Debug.WriteLine("PutAsync data = " + await data?.ReadAsStringAsync());
            }

            System.Diagnostics.Debug.WriteLine("PutAsync token = " + token);
            System.Diagnostics.Debug.WriteLine("PutAsync HttpClient = " + httpClient);


            RequestResponseItem responseItem = null;


            if (!string.IsNullOrWhiteSpace(apiUrl))
            {

                var client = CreateHttpClient(httpClient, token);

                try
                {
                    var response = await client.PutAsync(apiUrl, data);

                    string json = await response.Content.ReadAsStringAsync();
                    var responseStatus = response.StatusCode;

                    responseItem = new RequestResponseItem(responseStatus, json);


                    System.Diagnostics.Debug.WriteLine("httpcode = " + responseStatus);
                    System.Diagnostics.Debug.WriteLine("response = " + json);
                }
                catch (Exception e)
                {


                    System.Diagnostics.Debug.WriteLine("PutAsync Exception = " + e.ToString());
                    System.Diagnostics.Debug.WriteLine("httpcode = null");
                    System.Diagnostics.Debug.WriteLine("response = " + e.ToString());
                    var properties = new Dictionary<string, string>
                    {
                        { "apiUrl", apiUrl },
                    };
                    Crashes.TrackError(e, properties);
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
            System.Diagnostics.Debug.WriteLine("\nGetByteArray");
            System.Diagnostics.Debug.WriteLine("GetAsync apiUrl = " + apiUrl);
            System.Diagnostics.Debug.WriteLine("GetAsync HttpClient = " + httpClient);

            RequestResponseItem responseItem = null;

            if (!string.IsNullOrWhiteSpace(apiUrl))
            {

                var client = CreateHttpClient(httpClient, header: header);

                try
                {

                    var response = await client.GetAsync(apiUrl);

                    var byteArray = await response.Content.ReadAsByteArrayAsync();
                    var responseStatus = response.StatusCode;

                    responseItem = new RequestResponseItem(responseStatus, null, byteArray);

                    System.Diagnostics.Debug.WriteLine("ResposeCode = " + responseStatus);
                    System.Diagnostics.Debug.WriteLine("byteArray = " + byteArray);
                    System.Diagnostics.Debug.WriteLine("byteArray Lenght KB = " + byteArray.Length / 1024);
                }
                catch (Exception e)
                {


                    System.Diagnostics.Debug.WriteLine("GetByteArray Exception = " + e.ToString());
                    System.Diagnostics.Debug.WriteLine("ResposeCode = null");
                    System.Diagnostics.Debug.WriteLine("message = " + e.ToString());
                    var properties = new Dictionary<string, string>
                    {
                        { "apiUrl", apiUrl },
                    };
                    Crashes.TrackError(e, properties);
                }
            }
            else
            {
                throw new HttpRequestUrlEmptyExceptionEx();
            }

            return responseItem;
        }

        HttpClient CreateHttpClient(HttpClient httpClient = null, string token = null, bool header = true)
        {
            var httpClientInstance = httpClient;
            if (header)
            {
                if (httpClient == null)
                {
                    httpClientInstance = new HttpClient();
                }

                var authHeader = httpClientInstance.DefaultRequestHeaders;
                authHeader.Authorization = null;

                var authorization = authHeader.Authorization;
                if (!string.IsNullOrWhiteSpace(token) && authorization == null)
                {
                    httpClientInstance.DefaultRequestHeaders.Add("Authorization", "token " + token);

                }


                IEnumerable<string> values;
                if (!authHeader.TryGetValues("clientId", out values))
                {
                    httpClientInstance.DefaultRequestHeaders.Add("clientId", _clientId);
                }

                if (!authHeader.TryGetValues("clientVersione", out values))
                {
                    httpClientInstance.DefaultRequestHeaders.Add("clientVersione", _clientVersione);
                }

                if (!authHeader.Accept.Contains(_mediaTypeWithQualityHeaderValue))
                {
                    httpClientInstance.DefaultRequestHeaders.Accept.Add(_mediaTypeWithQualityHeaderValue);
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
