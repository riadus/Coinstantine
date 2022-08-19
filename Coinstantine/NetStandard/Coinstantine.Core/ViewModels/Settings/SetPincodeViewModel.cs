using System;
using System.Threading.Tasks;
using Coinstantine.Core.Services;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Auth;
using Coinstantine.Domain.Interfaces.Translations;

namespace Coinstantine.Core.ViewModels.Settings
{
    public class SetPincodeViewModel : BaseViewModel<PincodeViewModel.PincodeType>
    {
        private readonly IProfileProvider _profileProvider;
        private readonly IBiometricsAuthenticator _biometricsAuthenticator;
        private readonly ILogoutService _logoutService;
        public PincodeViewModel.PincodeType PincodeType { get; private set; }
        private PincodeViewModel.PincodeType _sessionPincodeType;
        private UserProfile _userProfile;
        private int _numberOfAttempts;
        private const int LimitOfAttempts = 3;

        public SetPincodeViewModel(IAppServices appServices,
                                   IProfileProvider profileProvider,
                                   IBiometricsAuthenticator biometricsAuthenticator,
                                   PincodeViewModel pincodeViewModel,
                                   ILogoutService logoutService) : base(appServices)
        {
            _biometricsAuthenticator = biometricsAuthenticator;
            _profileProvider = profileProvider;
            PincodeViewModel = pincodeViewModel;
            _logoutService = logoutService;
        }

        public PincodeViewModel PincodeViewModel { get; private set; }
        public bool ShowNavigationBar { get; private set; }
        public override void Prepare(PincodeViewModel.PincodeType parameter)
        {
            PincodeType = parameter;
        }

        public override async Task Initialize()
        {
            _userProfile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
            ShowNavigationBar = PincodeType == PincodeViewModel.PincodeType.ResetPinCode;
            _numberOfAttempts = _userProfile.InvalidPincodeAttempts;
            _sessionPincodeType = PincodeType == PincodeViewModel.PincodeType.ResetPinCode ? PincodeViewModel.PincodeType.CheckPinCode : PincodeType;
            await PincodeViewModel.SetPincodeType(_sessionPincodeType).ConfigureAwait(false);
            PincodeViewModel.PinCodeCompleted += PincodeViewModel_PinCodeCompleted;
            PincodeViewModel.AuthenticatedViaBiometrics += PincodeViewModel_AuthenticatedViaBiometrics;
            if (_sessionPincodeType == PincodeViewModel.PincodeType.CheckPinCode && _userProfile.InvalidPincodeAttempts > 0)
            {
                PincodeViewModel.SetError($"{GetChance()}");
            }
        }

        public override async void ViewAppeared()
        {
            base.ViewAppeared();
            if (_sessionPincodeType == PincodeViewModel.PincodeType.CheckPinCode && await _biometricsAuthenticator.HasBiometricsCapability().ConfigureAwait(false) && _userProfile.UseBiometricsToLogin)
            {
                await PincodeViewModel.BiometricsAuthentication().ConfigureAwait(false);
            }
        }

        private async void PincodeViewModel_PinCodeCompleted(object sender, PincodeViewModel.PinCodeEventArgs e)
        {
            switch(_sessionPincodeType)
            {
                case PincodeViewModel.PincodeType.ResetPinCode:
                    await ResetPincode(e).ConfigureAwait(false);
                    break;
                case PincodeViewModel.PincodeType.SetPinCode:
                    await SetPincode(e).ConfigureAwait(false);
                    break;
                case PincodeViewModel.PincodeType.CheckPinCode:
                    await CheckPincode(e).ConfigureAwait(false);
                    break;
            }
        }

        private async Task ResetPincode(PincodeViewModel.PinCodeEventArgs e)
        {
            await _profileProvider.SetPinCode(e.PinCode, e.FingerPrintEnabled).ConfigureAwait(false);
            PincodeViewModel.EraseInput();
            Close(this);
        }

        private async Task SetPincode(PincodeViewModel.PinCodeEventArgs e)
        {
            await _profileProvider.SetPinCode(e.PinCode, e.FingerPrintEnabled).ConfigureAwait(false);
            await _profileProvider.Login().ConfigureAwait(false);
            AppNavigationService.ShowHomePage();
            PincodeViewModel.EraseInput();
            return;
        }

        private async Task CheckPincode(PincodeViewModel.PinCodeEventArgs e)
        {
            if (_profileProvider.VerifyPincode(e.PinCode, _userProfile))
            {
                PincodeViewModel.IsLocked = false;
                await Authenticated().ConfigureAwait(false);
            }
            else
            {
                _numberOfAttempts = _userProfile.InvalidPincodeAttempts++;
                PincodeViewModel.IsLocked = true;
                PincodeViewModel.SetError($"{Translate(TranslationKeys.Pincode.WrongPincode)} {GetChance()}");
                if (_userProfile.InvalidPincodeAttempts >= LimitOfAttempts)
                {
                    await _logoutService.TooManyWrongPincodes().ConfigureAwait(false);
                    PincodeViewModel.EraseInput();
                    return;
                }
                await _profileProvider.SaveUserProfile(_userProfile).ConfigureAwait(false);
                PincodeViewModel.EraseInput();
            }
        }

        async void PincodeViewModel_AuthenticatedViaBiometrics(object sender, EventArgs e)
        {
            await Authenticated().ConfigureAwait(false);
        }

        private async Task Authenticated()
        {
            if (PincodeType == PincodeViewModel.PincodeType.ResetPinCode)
            {
                if (_sessionPincodeType == PincodeViewModel.PincodeType.CheckPinCode)
                {
                    PincodeViewModel.EraseInput();
                    _sessionPincodeType = PincodeViewModel.PincodeType.ResetPinCode;
                    PincodeViewModel.FingerPrintEnabled = _userProfile?.UseBiometricsToLogin ?? false;
                    await PincodeViewModel.SetPincodeType(_sessionPincodeType).ConfigureAwait(false);
                    return;
                }
            }
            await _profileProvider.Login().ConfigureAwait(false);
            AppNavigationService.ShowHomePage();
            PincodeViewModel.EraseInput();
        }

        private string GetChance()
        {
            if (_numberOfAttempts == 0)
            {
                return Translate(TranslationKeys.Pincode.SecondChance);
            }
            if (_numberOfAttempts >= 1)
            {
                return Translate(TranslationKeys.Pincode.LastChance);
            }
            return string.Empty;
        }
    }
}
