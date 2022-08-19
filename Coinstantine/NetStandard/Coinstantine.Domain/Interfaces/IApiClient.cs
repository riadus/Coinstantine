using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Coinstantine.Domain.Interfaces
{
    public interface IApiClient
    {
        Task<T> GetAsync<T>(string url);
        Task<byte[]> GetBytesAsync(string url);
        Task<T> PostAsync<T, TContent>(string url, TContent content) where TContent : class;
		Task<T> PutAsync<T, TContent>(string url, TContent content) where TContent : class;
        Task<(HttpStatusCode, T)> GetWithStatusAsync<T>(string url, HttpMethod httpMethod);
        Task PostFireAndForgetAsync<TContent>(string url, TContent content) where TContent : class;
		Task PostEmptyAsync(string url);
        Task<bool> DeleteAsync(string url);
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    }
}
