using Coinstantine.Core.Services;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces.Translations;
using MvvmCross.ViewModels;
using MvvmCross;
using MvvmCross.Commands;
using Coinstantine.Domain.Documents;
using System.Linq;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Core.UIServices;

namespace Coinstantine.Core.ViewModels.Settings
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly IDocumentProvider _documentProvider;


        public SettingsViewModel(IAppServices appServices,
                                 IDocumentProvider documentProvider) : base(appServices)
        {
            _documentProvider = documentProvider;

            TitleIcon = "cog";
            Title = TranslationKeys.MainMenu.Settings;

            var languageViewModel = Mvx.Resolve<SettingItemViewModel>();
            languageViewModel.IconTitle = "flag";
            languageViewModel.Title = TranslationKeys.Settings.ChangeLanguage;
            languageViewModel.SelectedCommand = new MvxCommand(AppNavigationService.ShowChangeLanguage);

            var resetPincodeViewModel = Mvx.Resolve<SettingItemViewModel>();
            resetPincodeViewModel.IconTitle = "undo";
            resetPincodeViewModel.Title = TranslationKeys.Settings.ResetPincode;
            resetPincodeViewModel.SelectedCommand = new MvxCommand(AppNavigationService.ShowResetPincode);
                                 
            var aboutViewModel = Mvx.Resolve<SettingItemViewModel>();
            aboutViewModel.IconTitle = "about";
            aboutViewModel.Title = TranslationKeys.Settings.About;
            aboutViewModel.SelectedCommand = new MvxCommand(AppNavigationService.ShowAboutPage);

            var tcViewModel = Mvx.Resolve<SettingItemViewModel>();
            tcViewModel.IconTitle = "file-alt";
            tcViewModel.Title = TranslationKeys.Settings.TermsAndConditions;

            Settings = new MvxObservableCollection<SettingItemViewModel>
            {
                languageViewModel,
                resetPincodeViewModel,
                aboutViewModel,
            };
            SelectedSettingCommand = new MvxCommand<SettingItemViewModel>(item => SelectedSetting = item);
        }

        public MvxObservableCollection<SettingItemViewModel> Settings { get; private set; }

        public override async void ViewAppearing()
        {
            var documents = await _documentProvider.LoadDocuments().ConfigureAwait(false);
            foreach(var doc in documents.Where(x => x.DocumentAvailable || x.FileType == FileApplicationType.Web))
            {
                var documentViewModel = Mvx.Resolve<DocumentViewModel>();
                documentViewModel.Document = doc;
                if (!Settings.Any(x => x is DocumentViewModel docViewModel && docViewModel.Document.DocumentType == doc.DocumentType))
                {
                    Settings.Add(documentViewModel);
                }
            }
            base.ViewAppearing();
        }


        public IMvxCommand<SettingItemViewModel> SelectedSettingCommand { get; }

        private SettingItemViewModel _selectedSetting;

        public SettingItemViewModel SelectedSetting
        {
            get
            {
                return _selectedSetting;
            }

            set
            {
                _selectedSetting = value;
                if (_selectedSetting?.SelectedCommand != null)
                {
                    _selectedSetting.SelectedCommand.Execute();
                }
                _selectedSetting = null;
                RaisePropertyChanged();
            }
        }
    }

    public class DocumentViewModel : SettingItemViewModel
    {
        private readonly IAppOpener _appOpener;
        private readonly IDocumentProvider _documentProvider;
        private Document _document;

        public DocumentViewModel(IAppServices appServices,
                                 IAppOpener appOpener,
                                 IDocumentProvider documentProvider,
                                 IProfileProvider profileProvider) : base(appServices, profileProvider)
        {
            _appOpener = appOpener;
            _documentProvider = documentProvider;
            IconTitle = "file-alt";
            SelectedCommand = new MvxCommand(OpenDocument);
        }

        public Document Document
        {
            get => _document; 
            set
            {
                _document = value;
                Title = GetTranslationKey(_document.DocumentType);
            }
        }
        private TranslationKey GetTranslationKey(DocumentType documentType)
        {
            switch (documentType)
            {
                case DocumentType.WhitePaper:
                    return TranslationKeys.Documents.WhitePaper;
                case DocumentType.TermsAndServices:
                    return TranslationKeys.Documents.TermsAndServices;
                case DocumentType.PrivacyPolicy:
                    return TranslationKeys.Documents.PrivacyPolicy;
            }
            return documentType.ToString();
        }

        private void OpenDocument()
        {
            if (Document.FileType == FileApplicationType.Web)
            {
                _appOpener.OpenLink(Document.Filename);
            }
            else
            {
                var loadedDocument = _documentProvider.LoadFullDocument(Document);
                _appOpener.OpenDocument(loadedDocument.Path);
            }
        }
    }
}
