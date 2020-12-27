using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace RaveDotNet.Helpers
{
    public class ApiClient : IApiClient
    {
        private static ApiClient _apiClient;
        private ApiClient()
        {

        }

        public static ApiClient GetApiClient() {
            return _apiClient ?? (_apiClient = new ApiClient());
        }

        public async Task<TClass> Get<TClass>(string url, Dictionary<string, string> headers = null, AuthenticationHeaderValue authorization = null)
           where TClass : class
        {
            using (var client = new HttpClient())
            {
                if (authorization != null) client.DefaultRequestHeaders.Authorization = authorization;
                var message = new HttpRequestMessage(HttpMethod.Get, url);
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        message.Headers.Add(header.Key, header.Value);
                    }
                }
                var response = await client.SendAsync(message).ConfigureAwait(false);
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode) {
                    throw new Exception("Remote Server did not return success status code (see inner)",
                        new Exception(responseString));
                }

                try
                {
                    return JsonConvert.DeserializeObject<TClass>(responseString);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error when attempting to convert to json (see inner)", new Exception(ex.Message, new Exception(responseString)));
                }
            }
        }

        public async Task<TClass> Post<TClass>(string url, HttpContent body, Dictionary<string, string> headers = null, AuthenticationHeaderValue authorization = null)
           where TClass : class
        {
            using (var client = new HttpClient())
            {
                if (authorization != null) client.DefaultRequestHeaders.Authorization = authorization;
                var message = new HttpRequestMessage(HttpMethod.Post, url);
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        message.Headers.Add(header.Key, header.Value);
                    }
                }
                message.Content = body;
                var response = await client.SendAsync(message).ConfigureAwait(false);
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        return JsonConvert.DeserializeObject<TClass>(responseString);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error when attempting to convert to json (see inner)", new Exception(ex.Message, new Exception(responseString)));
                    }
                }

                try
                {
                    return JsonConvert.DeserializeObject<TClass>(responseString);
                }
                catch (Exception ex)
                {
                    throw new Exception("Remote Server did not return success status code (see inner)", new Exception(ex.Message, new Exception(responseString)));
                }
            }
        }

        public async Task<TClass> Put<TClass>(string url, HttpContent body, Dictionary<string, string> headers = null, AuthenticationHeaderValue authorization = null)
           where TClass : class
        {
            using (var client = new HttpClient())
            {
                if (authorization != null) client.DefaultRequestHeaders.Authorization = authorization;
                var message = new HttpRequestMessage(HttpMethod.Put, url);
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        message.Headers.Add(header.Key, header.Value);
                    }
                }
                message.Content = body;
                var response = await client.SendAsync(message).ConfigureAwait(false);
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode) {
                    throw new Exception("Remote Server did not return success status code (see inner)",
                        new Exception(responseString));
                }

                try
                {
                    return JsonConvert.DeserializeObject<TClass>(responseString);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error when attempting to convert to json (see inner)", new Exception(ex.Message, new Exception(responseString)));
                }
            }
        }

        public async Task<TClass> Delete<TClass>(string url, Dictionary<string, string> headers = null, AuthenticationHeaderValue authorization = null)
           where TClass : class
        {
            using (var client = new HttpClient())
            {
                if (authorization != null) client.DefaultRequestHeaders.Authorization = authorization;
                var message = new HttpRequestMessage(HttpMethod.Delete, url);
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        message.Headers.Add(header.Key, header.Value);
                    }
                }
                var response = await client.SendAsync(message).ConfigureAwait(false);
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                
                if (!response.IsSuccessStatusCode) {
                    throw new Exception("Remote Server did not return success status code (see inner)",
                        new Exception(responseString));
                }

                try
                {
                    return JsonConvert.DeserializeObject<TClass>(responseString);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error when attempting to convert to json (see inner)", new Exception(ex.Message, new Exception(responseString)));
                }
            }
        }
        public static StringContent GetJsonContent<TSource>(TSource obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.Unicode, "application/json");
        }
    }
}