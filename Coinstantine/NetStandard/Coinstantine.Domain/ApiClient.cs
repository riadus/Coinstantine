using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Coinstantine.Common;
using Coinstantine.Domain.Exceptions;
using Coinstantine.Domain.Extensions;
using Coinstantine.Domain.Interfaces;

namespace Coinstantine.Domain
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        protected readonly ITokenProvider _tokenProvider;
        protected readonly IEndpointProvider _endpointProvider;
        private readonly IAnalyticsTracker _analyticsTracker;

        private const int _defaultRequestTimeOut = 180;

        public ApiClient(ITokenProvider tokenProvider, 
                         IEndpointProvider endpointProvider,
                         IAnalyticsTracker analyticsTracker)
        {
            _tokenProvider = tokenProvider;
            _endpointProvider = endpointProvider;
            _analyticsTracker = analyticsTracker;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(endpointProvider.Endpoint),
                Timeout = TimeSpan.FromSeconds(_defaultRequestTimeOut)
            };
        }

        public virtual async Task<T> GetAsync<T>(string url)
        {
            var httpContent = await GetContent(url, HttpMethod.Get).ConfigureAwait(false);
            var content = await httpContent.ReadAsStringAsync().ConfigureAwait(false);

            return content.DeserializeTo<T>();
        }

        public virtual async Task<byte[]> GetBytesAsync(string url)
        {
            try
            {
                var httpContent = await GetContent(url, HttpMethod.Get).ConfigureAwait(false);
                var bytes = await httpContent.ReadAsByteArrayAsync().ConfigureAwait(false);
                return bytes;
            }
            catch(Exception e)
            {
                throw;
            }
        }

        private async Task<T> SendAsync<T, TContent>(string url, TContent content, HttpMethod httpMethod) where TContent : class
        {
            var request = new HttpRequestMessage(httpMethod, url);
            await ApplyHeader(request).ConfigureAwait(false);

            if (content != null)
            {
                var contentSerialized = content.Serialize();
                request.Content = new StringContent(contentSerialized);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                Debug.WriteLine($"API POST - {url}\n{contentSerialized}");
            }
            else
            {
                request.Content = new StringContent(string.Empty);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                Debug.WriteLine($"API POST - {url}");
            }

            var sw = new Stopwatch();
            sw.Start();
            try
            {
                var response = await SendAsync(request).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var result = responseContent.DeserializeTo<T>();
                    return result;
                }
                throw new HttpRequestException();
            }
            catch (TaskCanceledException tce)
            {
                Debug.WriteLine($"HttpClient Get request {url} timed out");
                throw new HttpRequestException("Request timed out", tce);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"HttpClient Post request {url} exception {ex}");
                throw;
            }
            finally
            {
                sw.Stop();
                _analyticsTracker.TrackApiCall(url, sw.ElapsedMilliseconds);
                Debug.WriteLine($"HttpClient Post request {url} took {sw.ElapsedMilliseconds} ms");
            }
        }

        private async Task<HttpContent> GetContent(string url, HttpMethod httpMethod)
        {
            var request = new HttpRequestMessage(httpMethod, url);
            await ApplyHeader(request).ConfigureAwait(false);
            var response = await SendAsync(request).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                return response.Content;
            }

            throw new HttpResponseException(response);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            var url = request.RequestUri.ToString();
            var sw = new Stopwatch();
            sw.Start();
            try
            {
                return await _httpClient.SendAsync(request).ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine($"Http GET request {url} timed out");
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Http GET request {url} exception {ex}");
                throw;
            }
            finally
            {
                sw.Stop();
                _analyticsTracker.TrackApiCall(url, sw.ElapsedMilliseconds);
                Debug.WriteLine($"Http GET request {url} took {sw.ElapsedMilliseconds} ms");
            }
        }

        public virtual async Task<(HttpStatusCode, T)> GetWithStatusAsync<T>(string url, HttpMethod httpMethod)
        {
            var request = new HttpRequestMessage(httpMethod, url);
            await ApplyHeader(request).ConfigureAwait(false);
            var response = await SendAsync(request).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var result = responseContent.DeserializeTo<T>();
                return (response.StatusCode, result);
            }
            return (response.StatusCode, default(T));
        }


        private async Task ApplyHeader(HttpRequestMessage request)
        {
            request.Headers.Add("client_id", _endpointProvider.ClientId);
            request.Headers.Add("secret", _endpointProvider.Secret);
            var token = await _tokenProvider.GetToken().ConfigureAwait(false);
            if (!token.IsNullOrEmpty())
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public virtual Task PostFireAndForgetAsync<TContent>(string url, TContent content) where TContent : class
		{
			return SendAsync<object, TContent>(url, content, HttpMethod.Post);
		}

        public virtual Task PostEmptyAsync(string url)
		{
            return SendAsync<object, string>(url, null, HttpMethod.Post);
		}

        public virtual Task<T> PutAsync<T, TContent>(string url, TContent content) where TContent : class
        {
            return SendAsync<T, TContent>(url, content, HttpMethod.Put);
        }

        public virtual async Task<bool> DeleteAsync(string url)
        {
            var result =  await GetWithStatusAsync<object>(url, HttpMethod.Delete).ConfigureAwait(false);
            return IsSuccessCode(result.Item1);
        }

        private bool IsSuccessCode(HttpStatusCode httpStatusCode)
        {
            return ((int)httpStatusCode >= 200) && ((int)httpStatusCode <= 299);
        }

        public virtual Task<T> PostAsync<T, TContent>(string url, TContent content) where TContent : class
        {
            return SendAsync<T, TContent>(url, content, HttpMethod.Post);
        }
    }
}
