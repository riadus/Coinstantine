using System.Linq;
using System.Threading.Tasks;
using Coinstantine.Core.Services;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Translations;
using MvvmCross.ViewModels;

namespace Coinstantine.Core.ViewModels.Settings
{
    public class ChangeLanguageViewModel : BaseViewModel
    {
        private readonly IProfileProvider _profileProvider;


        public ChangeLanguageViewModel(IAppServices appServices,
                                       IProfileProvider profileProvider) : base(appServices)
        {
            Languages = new MvxObservableCollection<LanguageItemViewModel>();
            Title = TranslationKeys.Settings.ChangeLanguage;
            TitleIcon = "flag";
            _profileProvider = profileProvider;
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            TrackPage("Change language page");
        }

        public override async void Prepare()
        {
            base.Prepare();
            var allLanguages = await AppServices.TranslationService.GetTranslationsForKey(TranslationKeys.General.OwnLanguage).ConfigureAwait(false);
            var currentLanguage = AppServices.TranslationService.CurrentLanguage;
            Languages = new MvxObservableCollection<LanguageItemViewModel>()
            {
                new LanguageItemViewModel(AppServices, allLanguages.First(x => x.Language == currentLanguage))
                {
                    IsSelected = true
                }
            };
            Languages.AddRange(allLanguages.Where(x => x.Language != currentLanguage)
                                           .OrderBy(x => x.Language)
                                           .Select(translation => new LanguageItemViewModel(AppServices, translation)));
            _selectedLanguage = Languages.First(x => x.IsSelected);
            RaisePropertyChanged(nameof(Languages));
        }

        public void ReorderList()
        {
            Languages.Remove(SelectedLanguage);
            Languages.Insert(0, SelectedLanguage);
            RaisePropertyChanged(nameof(Languages));
        }

        public MvxObservableCollection<LanguageItemViewModel> Languages { get; private set; }

        private LanguageItemViewModel _selectedLanguage;
        public LanguageItemViewModel SelectedLanguage
        {
            get
            {
                return _selectedLanguage;
            }

            set
            {
                if (value != null)
                {
                    _selectedLanguage.IsSelected = false;
                    _selectedLanguage = value;
                    _selectedLanguage.IsSelected = true;
                    UpdateLanguage();
                }
            }
        }

        private async Task UpdateLanguage()
        {
            await AppServices.TranslationService.ChangueLanguage(_selectedLanguage.Language).ConfigureAwait(false);
            RaisePropertyChanged(nameof(Title));
        }
    }

    public class LanguageItemViewModel : BaseViewModel
    {
        private readonly Translation _translation;

        public LanguageItemViewModel(IAppServices appServices, Translation translation) : base(appServices)
        {
            _translation = translation;
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
        public string Language => _translation.Language;
        public string LanguageText => _translation.Value;
    }
}
