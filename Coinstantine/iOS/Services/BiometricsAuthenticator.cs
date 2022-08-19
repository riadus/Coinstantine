using System;
using System.Threading.Tasks;
using Foundation;
using LocalAuthentication;
using Coinstantine.Common.Attributes;
using Coinstantine.Core.Services;
using UIKit;

namespace Coinstantine.iOS.Services
{
    [RegisterInterfaceAsDynamic]
    public class BiometricsAuthenticator : NSObject, IBiometricsAuthenticator
    {
        private readonly LAContext _context;
        private readonly bool _hasBiometricsCapability;
        private readonly string _biometricsTechnologyName;

        public BiometricsAuthenticator()
        {
            _context = new LAContext();
            _hasBiometricsCapability = _context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out NSError authError);
            _biometricsTechnologyName = UIDevice.CurrentDevice.CheckSystemVersion(11, 0) && _context.BiometryType == LABiometryType.FaceId ? "Face ID" : "Touch ID";
        }

        public async Task<bool> Authenticate(string reason)
        {
            var tcs = new TaskCompletionSource<bool>();
            var replyHandler = new LAContextReplyHandler((success, error) =>
            {
                tcs.SetResult(success);
            });
            _context.EvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, reason, replyHandler);

            return await tcs.Task.ConfigureAwait(false);
        }

        public Task<string> BiometricsTechnologyName()
        {
            return Task.FromResult(_biometricsTechnologyName);
        }

        public Task<bool> HasBiometricsCapability()
        {
            return Task.FromResult(_hasBiometricsCapability);
        }
    }
}
