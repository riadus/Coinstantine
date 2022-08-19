using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coinstantine.Core.Services;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Translations;
using MvvmCross.Commands;

namespace Coinstantine.Core.ViewModels.Settings
{
    public class PincodeViewModel : BaseViewModel
    {
        private List<Action<string>> _inputs;
        private string _input;
        private readonly IProfileProvider _profileProvider;
        private readonly IBiometricsAuthenticator _biometricsAuthenticator;
        private PincodeType _pincodeType;

		public PincodeViewModel(IAppServices appServices,
                                IProfileProvider profileProvider,
                                IBiometricsAuthenticator biometricsAuthenticator) : base(appServices)
        {
            _input = string.Empty;
            PinCommand = new MvxAsyncCommand<Pin>(PinPushed);
            _inputs = new List<Action<string>>{
                s => Pin1Label = s,
                s => Pin2Label = s,
                s => Pin3Label = s,
                s => Pin4Label = s
            };
            _profileProvider = profileProvider;
            _biometricsAuthenticator = biometricsAuthenticator;
        }
        public string InformationLabel { get; private set; }
        public string Error { get; private set; }
		public string LockLabel => IsLocked ? _lockedLabel : _unlockedLabel;
		bool _isLocked;

        public async Task SetPincodeType(PincodeType pincodeType)
        {
            _pincodeType = pincodeType;
            ShowFingerPrintButton = await _biometricsAuthenticator.HasBiometricsCapability().ConfigureAwait(false);
            IsLocked = pincodeType == PincodeType.CheckPinCode;
            await SetStrings().ConfigureAwait(false);
            RaiseAllPropertiesChanged();
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            TrackPage("Pincode page");
        }

		public bool IsLocked
		{
			get
			{
				return _isLocked;
			}

			set
			{
				SetProperty(ref _isLocked, value);
				RaisePropertyChanged(nameof(LockLabel));
			}
		}

        private readonly string _lockedLabel = "lock";
        private readonly string _unlockedLabel = "unlock-alt";

        public string FingerPrintText => "fingerprint";
        public string DeleteText => "arrow-alt-circle-left";

        private async Task SetStrings()
        {
            switch(_pincodeType)
            {
                case PincodeType.CheckPinCode:
                    InformationLabel = Translate(TranslationKeys.Pincode.CheckPincode);
                    break;
                case PincodeType.ResetPinCode:
                    InformationLabel = Translate(TranslationKeys.Pincode.SetNewPincode);
                    break;
                case PincodeType.SetPinCode:
					InformationLabel = Translate(TranslationKeys.Pincode.SetPincode);
                    if(ShowFingerPrintButton)
                    {
						InformationLabel += $".\n{Translate(TranslationKeys.Pincode.UseBiometrics)} {await _biometricsAuthenticator.BiometricsTechnologyName().ConfigureAwait(false)}";
                    }
                    break;
            }
        }
        public event EventHandler<PinCodeEventArgs> PinCodeCompleted;
        public event EventHandler AuthenticatedViaBiometrics;

        private void OnPinCodeCompleted(string input)
        {
            PinCodeCompleted?.Invoke(this, new PinCodeEventArgs(input, FingerPrintEnabled));
        }

        private void OnAuthenticaedViaBiometrics()
        {
            AuthenticatedViaBiometrics?.Invoke(this, EventArgs.Empty);
            EraseInput();
        }

        public void EraseInput()
        {
            _input = string.Empty;
            foreach(var input in _inputs)
            {
                input(string.Empty);
            }
        }

        public void SetError(string errorMessage)
        {
            Error = errorMessage;
            RaisePropertyChanged(nameof(Error));
        }

        private async Task PinPushed(Pin pin)
        {
            switch(pin)
            {
                case Pin.Pin0:
                case Pin.Pin1:
                case Pin.Pin2:
                case Pin.Pin3:
                case Pin.Pin4:
                case Pin.Pin5:
                case Pin.Pin6:
                case Pin.Pin7:
                case Pin.Pin8:
                case Pin.Pin9:
					Write(pin);
                    break;
                case Pin.Del:
                    Delete();
                    break;
                case Pin.FingerPrint:
                    await FingerPrint();
                    break;
            }
        }

        private async Task FingerPrint()
        {
            switch(_pincodeType)
            {
                case PincodeType.CheckPinCode:
                    await BiometricsAuthentication().ConfigureAwait(false);
                    break;
                case PincodeType.SetPinCode:
                case PincodeType.ResetPinCode:
                    FingerPrintEnabled = !FingerPrintEnabled;
                    break;
            }
        }

        private void Write(Pin pin)
        {
            if (_input.Length >= _inputs.Count) { return; }
            var pinString = ((int)pin).ToString();
            _inputs[_input.Length](pinString);

            _inputs[_input.Length]("*");
            _input += pinString;

            if(_input.Length == 4)
            {
                OnPinCodeCompleted(_input);
            }
        }

        public bool ShowFingerPrintButton { get; private set; }
        private bool _fingerPrintEnabled;

        public bool FingerPrintEnabled
        {
            get
            {
                return _fingerPrintEnabled;
            }

            set
            {
                _fingerPrintEnabled = value;
                RaisePropertyChanged();
            }
        }

        private void Delete()
        {
            if (_input.Length <= 0) { return; }
            _inputs[_input.Length - 1](string.Empty);
            _input = _input.Remove(_input.Length - 1);
            RaiseAllPropertiesChanged();
        }

        public async Task BiometricsAuthentication()
        {
            if (await _biometricsAuthenticator.HasBiometricsCapability().ConfigureAwait(false))
            {
                var authenticated = await _biometricsAuthenticator.Authenticate(Translate(TranslationKeys.Pincode.WhyUseBiometrics)).ConfigureAwait(false);
                if(authenticated)
                {
                    OnAuthenticaedViaBiometrics();
                }
            }
        }

        public IMvxCommand<Pin> PinCommand { get; }

        private string _pin1Label;
        public string Pin1Label
        {
            get
            {
                return _pin1Label;
            }

            private set
            {
                _pin1Label = value;
                RaisePropertyChanged();
            }
        }

        private string _pin2Label;
        public string Pin2Label
        {
            get
            {
                return _pin2Label;
            }

            private set
            {
                _pin2Label = value;
                RaisePropertyChanged();
            }
        }

        private string _pin3Label;
        public string Pin3Label
        {
            get
            {
                return _pin3Label;
            }

            private set
            {
                _pin3Label = value;
                RaisePropertyChanged();
            }
        }

        private string _pin4Label;
        public string Pin4Label
        {
            get
            {
                return _pin4Label;
            }

            private set
            {
                _pin4Label = value;
                RaisePropertyChanged();
            }
        }

        public bool FingerPrintToUnlock => _pincodeType == PincodeType.CheckPinCode;

        public enum Pin
        {
            Pin0,
            Pin1,
            Pin2,
            Pin3,
            Pin4,
            Pin5,
            Pin6,
            Pin7,
            Pin8,
            Pin9,
            Del,
            FingerPrint
        }

        public class PinCodeEventArgs : EventArgs
        {
            public PinCodeEventArgs(string pinCode, bool fingerPrintEnabled)
            {
                PinCode = pinCode;
                FingerPrintEnabled = fingerPrintEnabled;
            }
            public string PinCode { get; }
            public bool FingerPrintEnabled { get; }
        }

        public enum PincodeType
        {
            SetPinCode,
            CheckPinCode,
            ResetPinCode
        }
    }
}
