using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coinstantine.Data;
using Coinstantine.Common.Attributes;
using Coinstantine.Domain.Auth;
using Coinstantine.Domain.Extensions;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Auth;

namespace Coinstantine.Domain
{
    [RegisterInterfaceAsDynamic]
    public class TokenProvider : ITokenProvider
    {
        private const string AppName = "Coinstantine";
        private readonly IUnitOfWork _unitOfWork;

        public TokenProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task SaveToken(Tokens token)
        {
            var authenticationObject = ConvertAccessToken(token);
            await _unitOfWork.AuthenticationObjects.ReplaceAll(authenticationObject).ConfigureAwait(false);
        }

        public async Task<string> GetToken()
        {
            var authentificationObjects = await _unitOfWork.AuthenticationObjects.GetAsync(true).ConfigureAwait(false);
            return authentificationObjects.FirstOrDefault()?.AccessToken;
        }

        public async Task<string> GetRefreshToken()
        {
            var authentificationObjects = await _unitOfWork.AuthenticationObjects.GetAsync(true).ConfigureAwait(false);
            return authentificationObjects?.FirstOrDefault()?.RefreshToken;
        }

        public async Task<bool> HasExpiredToken()
        {
            var authentificationObjects = await _unitOfWork.AuthenticationObjects.GetAsync(true).ConfigureAwait(false);
            return authentificationObjects?.FirstOrDefault()?.ExpirationDate < DateTime.Now.ToUniversalTime();
        }

        private AuthenticationObject ConvertAccessToken(Tokens tokens)
        {
            var accessTokenParts = tokens.Token.Split('.');
            var tokenEncoded = accessTokenParts[1];

            var tokenBytes = Base64Url.Decode(tokenEncoded);
            var tokenDecoded = Encoding.UTF8.GetString(tokenBytes, 0, tokenBytes.Length);
            var token = tokenDecoded.DeserializeTo<JsonWebToken>();

            return new AuthenticationObject
            {
                AccessToken = tokens.Token,
                ExpirationDate = DateTimeFromUnixSeconds(token.Expiry),
                Email = token.Email,
                NameIdentifier = token.NameIdentifier,
                Role = token.Role,
                RefreshToken = tokens.RefreshToken,
                RefreshTokenExpirationDate = DateTimeFromUnixSeconds(tokens.ExpirationDate)
            };
        }

        private static DateTime DateTimeFromUnixSeconds(long secondsSince1970)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(secondsSince1970);
        }

        public async Task<bool> HasExpiredRefreshToken()
        {
            var authentificationObjects = await _unitOfWork.AuthenticationObjects.GetAsync(true).ConfigureAwait(false);
            return authentificationObjects?.FirstOrDefault()?.RefreshTokenExpirationDate < DateTime.Now.ToUniversalTime();
        }
    }
}
