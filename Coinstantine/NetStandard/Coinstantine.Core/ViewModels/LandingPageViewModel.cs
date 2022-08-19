using System.Threading.Tasks;
using Coinstantine.Common;
using Coinstantine.Core.Services;
using Coinstantine.Core.ViewModels.Settings;
using Coinstantine.Domain;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Auth;
using Coinstantine.Domain.Interfaces.Translations;
using MvvmCross.Commands;

namespace Coinstantine.Core.ViewModels
{
    public class LandingPageViewModel : BaseViewModel
    {
        public LandingPageViewModel(IAppServices appServices) : base(appServices)
        {
            LogonButtonCommand = new MvxCommand(GetStarted);
            CreateAccountButtonCommand = new MvxCommand(CreateAccount);
        }

        public string LoginButtonText => Translate(TranslationKeys.LandingPage.Login);
        public string CreateAccountButtonText => Translate(TranslationKeys.LandingPage.GetStarted);

        public IMvxCommand LogonButtonCommand { get; }
        public IMvxCommand CreateAccountButtonCommand { get; }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            TrackPage("Landing page");
        }

        private void GetStarted()
        {
            try
            {
                if (!CheckConnectivity())
                {
                    return;
                }
                AppNavigationService.ShowLoginPage();
            }
            catch(NotConnectedException)
            {
                ShowNoConnectionMessage();
            }
        }

        private void CreateAccount()
        {
            if (!CheckConnectivity())
            {
                return;
            }
            AppNavigationService.ShowCreateAccount();
        }
    }
}
