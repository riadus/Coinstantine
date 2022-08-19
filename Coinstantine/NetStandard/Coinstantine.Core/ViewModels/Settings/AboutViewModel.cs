using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coinstantine.Core.Services;
using Coinstantine.Core.UIServices;
using Coinstantine.Core.ViewModels.Generic;
using Coinstantine.Domain.Airdrops;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Translations;
using MvvmCross.ViewModels;

namespace Coinstantine.Core.ViewModels.Settings
{
    public class AboutViewModel : GenericInfoViewModel<object>
    {
        private readonly IAppOpener _appOpener;
        private readonly IAppVersion _appVersion;

        public AboutViewModel(IAppServices appServices, 
                              IProfileProvider profileProvider, 
                              IUserService userService, 
                              IGenericInfoItemViewModelConstructor itemInfoViewModelConstructor,
                              IAppOpener appOpener,
                              IAppVersion appVersion) 
                            : base(appServices, profileProvider, userService, itemInfoViewModelConstructor)
        {
            Title = TranslationKeys.Settings.About;
            TitleIcon = "about";
            _appOpener = appOpener;
            _appVersion = appVersion;
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            TrackPage("About page");
        }

        private string N => char.ConvertFromUtf32(0x1F1F3);
        private string L => char.ConvertFromUtf32(0x1F1F1);
        private string F => char.ConvertFromUtf32(0x1F1EB);
        private string R => char.ConvertFromUtf32(0x1F1F7);
        private string D => char.ConvertFromUtf32(0x1F1E9);
        private string Z => char.ConvertFromUtf32(0x1F1FF);

        public override async void Prepare(object parameter)
        {
            var ethereumNetwork = await _appVersion.GetEthereumNetwork().ConfigureAwait(false);
            var apiEnvironment = await _appVersion.GetApiEnvironment().ConfigureAwait(false);

            GenericInfoItems = new MvxObservableCollection<GenericInfoItemViewModel>(_itemInfoViewModelConstructor.Construct(new List<ItemInfo>{
                { (TranslationKeys.About.Versions, "", Display.Title) },
                { (TranslationKeys.About.AppVersion, _appVersion.Version) },
                { (TranslationKeys.About.BuildVersion, _appVersion.BuildVersion, Display.Grouped) },
                { (TranslationKeys.About.EthereumNetwork, ethereumNetwork.ToString()) },
                { (TranslationKeys.About.ApiEnvironment, apiEnvironment.ToString(), Display.Grouped) },
                { (TranslationKeys.About.Company, $"Coinstantine B.V. Company based in the Netherlands {N}{L}") },
                { (TranslationKeys.About.Developer, $"Riad Lakehal-Ayat {D}{Z}") },
                { (TranslationKeys.About.Designer, $"Lucie Hurel {F}{R}", Display.Grouped) },
                { (TranslationKeys.About.Contact, "info@coinstantine.io") },
                { (TranslationKeys.About.Technologies, "", Display.Title) },
                { (TranslationKeys.About.Framework, "Xamarin") },
                { (TranslationKeys.About.CrossPlatform, "MvvmCross", Display.Grouped) },
                { (TranslationKeys.About.Animations, "Lottie By AirBnB") },
                { (TranslationKeys.About.Tutorial, "Walkthrough by Xablu", Display.Grouped) },
                { (TranslationKeys.About.Backend, "Azure") },
                { (TranslationKeys.About.Blockchain, "NEthereum", Display.Grouped) }
            }));
        }

        public override string InfoTitle => Translate(TranslationKeys.Settings.About);
        public override bool ShowRegularBehaviourText => false;
        public override bool ShowPrincipalButton => true;
        protected override string AlternateBehaviourText => Translate(TranslationKeys.About.ContactUs);

        protected override Task SecondaryButtonAction()
        {
            return Task.FromResult(0);
        }

        protected override int GetRemainingTime()
        {
            return 0;
        }

        protected override Task PrincipalButtonAction()
        {
            if (!_appOpener.OpenEmailClient("info@coinstantine.io", "About Coinstantine"))
            {
                Alert(TranslationKeys.General.NoEmailClient);
            }
            return Task.FromResult(0);
        }
    }
}
