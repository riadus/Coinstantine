using System.Threading.Tasks;
using Coinstantine.Common.Attributes;
using Coinstantine.Core.Services;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Translations;
using Plugin.CurrentActivity;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;

namespace Coinstantine.Droid.UIServices
{
    [RegisterInterfaceAsDynamic]
    public class BiometricsAuthenticator : IBiometricsAuthenticator
    {
        private readonly IFingerprint _fingerprint;
        private readonly ITranslationService _translationService;

        public BiometricsAuthenticator(IFingerprint fingerprint,
                                       ITranslationService translationService)
        {
            _fingerprint = fingerprint;
            _translationService = translationService;
        }
        public Task<bool> HasBiometricsCapability()
        {
            return _fingerprint.IsAvailableAsync();
        }
        
        public async Task<string> BiometricsTechnologyName()
        {
            var authenticationType = await _fingerprint.GetAuthenticationTypeAsync().ConfigureAwait(false);
            switch(authenticationType)
            {
                case AuthenticationType.Face:
                    return _translationService.Translate(TranslationKeys.Pincode.AndroidFaceId);
                case AuthenticationType.Fingerprint:
                    return _translationService.Translate(TranslationKeys.Pincode.AndroidFingerprint);
                default:
                    return "";
            }
        }

        public async Task<bool> Authenticate(string reason)
        {
            var authenticationResult = await _fingerprint.AuthenticateAsync(reason).ConfigureAwait(false);
            return authenticationResult.Authenticated;
        }
    }
}