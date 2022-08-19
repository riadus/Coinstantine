using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coinstantine.Common.Attributes;
using Coinstantine.Data;
using Coinstantine.Domain.Auth.Models;
using Coinstantine.Domain.Extensions;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Auth;

namespace Coinstantine.Domain.Auth
{
    [RegisterInterfaceAsDynamic]
    public class AccountBackendService : IAccountBackendService
    {
        private readonly IApiClient _apiClient;
        private readonly ITokenProvider _tokenProvider;
        private readonly IEndpointProvider _endpointProvider;
        private readonly ITranslationService _translationService;

        public AccountBackendService(IApiClient apiClient,
                                     ITokenProvider tokenProvider,
                                     IEndpointProvider endpointProvider,
                                     ITranslationService translationService)
        {
            _apiClient = apiClient;
            _tokenProvider = tokenProvider;
            _endpointProvider = endpointProvider;
            _translationService = translationService;
        }

        public async Task<LoginStatus> Login(LoginModel loginModel)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "authentication");
                var base64bytes = Encoding.UTF8.GetBytes($"{loginModel.Username}:{loginModel.Password}");
                var base64 = Convert.ToBase64String(base64bytes);
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64);
                request.Headers.Add("client_id", _endpointProvider.ClientId);
                request.Headers.Add("secret", _endpointProvider.Secret);
                var response = await _apiClient.SendAsync(request).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    return (int)response.StatusCode == 423 ? LoginStatus.AccountNotConfirmed : LoginStatus.AuthenticationFailed;
                }
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var tokens = responseContent.DeserializeTo<Tokens>();
                await _tokenProvider.SaveToken(tokens).ConfigureAwait(false);
                return LoginStatus.AuthenticationSucceeded;
            }
            catch(Exception e)
            {
                return LoginStatus.AuthenticationFailed;
            }
        }

        public Task<AccountCorrect> CreateAccount(AccountCreationModel accountCreationModel)
        {
            return _apiClient.PostAsync<AccountCorrect, AccountCreationModel>("create-account", accountCreationModel);
        }

        public async Task<IEnumerable<Country>> GetCountries()
        {
            var lang = _translationService.CurrentLanguage;
            var countries = await _apiClient.GetAsync<IEnumerable<Country>>($"countries?locale={lang}").ConfigureAwait(false);
            return countries.Select(x => { x.Langugage = lang; return x; });
        }

        public Task ConfirmAccount(string email, string confirmationCode)
        {
            return _apiClient.PostFireAndForgetAsync<object>($"create-account/confirm/${email}?confirmationCode=${confirmationCode}", null);
        }

        public Task<AccountCorrect> IsAccountCorrect(AccountCreationModel accountCreationModel)
        {
            return _apiClient.PostAsync<AccountCorrect, AccountCreationModel>("create-account/check", accountCreationModel);
        }

        public Task RequestChangePassword(string email)
        {
            return _apiClient.PostFireAndForgetAsync<object>($"create-account/reset/username?email=${email}", null);
        }

        public Task ResetPassword(string userId, string confirmationCode)
        {
            return _apiClient.PostFireAndForgetAsync<object>($"create-account/change/password/${userId}?confirmationCode=${confirmationCode}", null);
        }

        public Task SendUsername(string email)
        {
            return _apiClient.PostFireAndForgetAsync<object>($"create-account/reset/username?email=${email}", null);
        }
    }

    [RegisterInterfaceAsLazySingleton]
    public class TokenRefreshService : ApiClient, ITokenRefreshService
    {
        private SemaphoreSlim _semaphore;
        public TokenRefreshService(ITokenProvider tokenProvider, 
                                   IEndpointProvider endpointProvider,
                                   IAnalyticsTracker analyticsTracker) : base(tokenProvider, endpointProvider, analyticsTracker)
        {
            _semaphore = new SemaphoreSlim(1, 1);
        }

        public async Task<bool> Refresh()
        {
            try
            {
                Debug.WriteLine($"Waiting to refresh : {_semaphore.CurrentCount}");
                await _semaphore.WaitAsync().ConfigureAwait(false);
                Debug.WriteLine($"My turn to refresh : {_semaphore.CurrentCount}");

                if (!await _tokenProvider.HasExpiredRefreshToken().ConfigureAwait(false) && !await _tokenProvider.HasExpiredToken().ConfigureAwait(false))
                {
                    return true;
                }
                var token = await _tokenProvider.GetToken();
                var refreshToken = await _tokenProvider.GetRefreshToken();
                var tokens = new Tokens
                {
                    Token = token,
                    RefreshToken = refreshToken
                };
                var contentSerialized = tokens.Serialize();
                var request = new HttpRequestMessage(HttpMethod.Post, "refreshToken")
                {
                    Content = new StringContent(contentSerialized)
                };
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                request.Headers.Add("client_id", _endpointProvider.ClientId);
                request.Headers.Add("secret", _endpointProvider.Secret);
                var response = await SendAsync(request).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var newTokens = responseContent.DeserializeTo<Tokens>();
                await _tokenProvider.SaveToken(newTokens).ConfigureAwait(false);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                _semaphore.Release();
                Debug.WriteLine($"Released semaphore : {_semaphore.CurrentCount}");
            }
        }
    }
}
