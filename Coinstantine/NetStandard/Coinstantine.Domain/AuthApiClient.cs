using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Coinstantine.Domain.Exceptions;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Auth;

namespace Coinstantine.Domain
{
    public class AuthApiClient : ApiClient
    {
        private readonly ILogoutService _logoutService;
        private readonly IApiClient _baseClient;
        private readonly ITokenRefreshService _tokenRefreshService;
        private readonly IAnalyticsTracker _analyticsTracker;

        public AuthApiClient(IApiClient baseClient,
                             ITokenRefreshService tokenRefreshService,
                             IAnalyticsTracker analyticsTracker,
                             ITokenProvider tokenProvider,
                             ILogoutService logoutService,
                             IEndpointProvider endpointProvider)
            : base(tokenProvider, endpointProvider, analyticsTracker)
        {
            _baseClient = baseClient;
            _tokenRefreshService = tokenRefreshService;
            _analyticsTracker = analyticsTracker;
            _logoutService = logoutService;
        }

        public override Task<T> GetAsync<T>(string url)
        {
            return RequestWithRetry(() => _baseClient.GetAsync<T>(url));
        }

        public override Task<byte[]> GetBytesAsync(string url)
        {
            return RequestWithRetry(() => _baseClient.GetBytesAsync(url));
        }

        public override Task<(System.Net.HttpStatusCode, T)> GetWithStatusAsync<T>(string url, HttpMethod httpMethod)
        {
            return RequestWithRetry(() => _baseClient.GetWithStatusAsync<T>(url, httpMethod));
        }

        public override Task<T> PostAsync<T, TContent>(string url, TContent content)
        {
            return RequestWithRetry(() => _baseClient.PostAsync<T, TContent>(url, content));
        }

        public override Task<bool> DeleteAsync(string url)
        {
            return RequestWithRetry(() => _baseClient.DeleteAsync(url));
        }

        public override Task PostEmptyAsync(string url)
        {
            return RequestWithRetry(() => _baseClient.PostEmptyAsync(url));
        }

        public override Task PostFireAndForgetAsync<TContent>(string url, TContent content)
        {
            return RequestWithRetry(() => _baseClient.PostFireAndForgetAsync(url, content));
        }

        public override Task<T> PutAsync<T, TContent>(string url, TContent content)
        {
            return RequestWithRetry(() => _baseClient.PutAsync<T, TContent>(url, content));
        }
        SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        async Task<T> RequestWithRetry<T>(Func<Task<T>> request)
        {
            var triesFailed = 0;
            while (triesFailed < 2)
            {
                try
                {
                    return await request().ConfigureAwait(false);
                }
                catch (HttpResponseException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    triesFailed++;

                    if (triesFailed > 1 || !await RefreshToken().ConfigureAwait(false))
                    {
                        await TokenExpired().ConfigureAwait(false);
                        return default(T);
                    }
                }
                catch (Exception ex)
                {
                    _analyticsTracker.TrackAppError(AnalyticsEventCategory.ApiCall, ex);
                    return default(T);
                }
            }
            return default(T);
        }

        async Task RequestWithRetry(Func<Task> request)
        {
            try
            {
                await request().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _analyticsTracker.TrackAppError(AnalyticsEventCategory.ApiCall, ex);
            }
        }

        private async Task<bool> RefreshToken()
        {
            return await _tokenRefreshService.Refresh().ConfigureAwait(false);
        }

        private Task TokenExpired()
        {
            return _logoutService.SessionExpired();
        }
    }
}