using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Coinstantine.Common;
using Coinstantine.Common.Attributes;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Translations;
using Coinstantine.Domain.Messages;
using MvvmCross.Plugin.Messenger;

namespace Coinstantine.Domain
{
    [RegisterInterfaceAsLazySingleton]
	public class TranslationService : ITranslationService
    {
		private readonly IProfileProvider _profileProvider;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMvxMessenger _mvxMessenger;
        private readonly IFixedJsonFilesReader _fixedJsonFilesReader;

		private const string DefaultLanguage = "en";

		private Dictionary<TranslationKey, string> _cachedTranslations;
		private Dictionary<TranslationKey, string> _defaultTranslations;
		private Dictionary<TranslationKey, string> _fixedTranslations;

		public TranslationService(IProfileProvider profileProvider,
		                          IUnitOfWork unitOfWork,
		                          IMvxMessenger mvxMessenger,
                                  IFixedJsonFilesReader fixedJsonFilesReader)
		{
			_profileProvider = profileProvider;
			_unitOfWork = unitOfWork;
            _mvxMessenger = mvxMessenger;
            _fixedJsonFilesReader = fixedJsonFilesReader;
			_cachedTranslations = new Dictionary<TranslationKey, string>();
			_defaultTranslations = new Dictionary<TranslationKey, string>();
			_fixedTranslations = new Dictionary<TranslationKey, string>();
		}
		public string CurrentLanguage { get; private set; }
		public string DeviceLanguage { get; set; }
        public CultureInfo CurrentCulture { get; private set; }

		public string Translate(TranslationKey translationKey, string defaultValue)
		{
			var translation = GetTranslation(translationKey);
            if (translation.IsNullOrEmpty())
            {
				return defaultValue;
            }
            return translation;
		}

		public string Translate(TranslationKey translationKey)
		{
            if (translationKey == TranslationKeys.General.Blank)
            {
                return string.Empty;
            }
			var translation = GetTranslation(translationKey);
			if (translation.IsNullOrEmpty())
			{
				return $"*{translationKey}*";
			}
			return translation;
		}

        private string GetTranslation(TranslationKey translationKey)
		{
			var translation = string.Empty;

			if (_cachedTranslations.TryGetValue(translationKey, out translation))
            {
                return translation;
            }
			if (_defaultTranslations.TryGetValue(translationKey, out translation))
            {
                return translation;
            }
			if (_fixedTranslations.TryGetValue(translationKey, out translation))
            {
                return translation;
            }
			return string.Empty;
		}

		public async Task LoadTranslations(bool forceLoadDefault)
		{
			await LoadFixedTranslations().ConfigureAwait(false);

			var profile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
			CurrentLanguage = profile?.PreferredLanguage ?? DeviceLanguage;
            TrySetCulture();

			if (!_defaultTranslations.Any() || forceLoadDefault)
			{
				var defaultTranslations = await _unitOfWork.Translations.GetAllAsync(x => x.Language == DefaultLanguage).ConfigureAwait(false);
				_defaultTranslations = ToFiltredDictionary(defaultTranslations);
			}
			var translations = await _unitOfWork.Translations.GetAllAsync(x => x.Language == CurrentLanguage).ConfigureAwait(false);
			_cachedTranslations = ToFiltredDictionary(translations);
		}

        private void TrySetCulture()
        {
            if (CurrentLanguage.IsNotNull())
            {
                try
                {
                    CurrentCulture = new CultureInfo(CurrentLanguage);
                    return;
                }
                catch
                {
                    if (CurrentLanguage.Contains("-") && !CurrentLanguage.StartsWith("-", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var language = CurrentLanguage.Split('-')[0];
                        CurrentCulture = new CultureInfo(language);
                        return;
                    }
                }
            }
            CurrentCulture = new CultureInfo(DefaultLanguage);
        }

        public async Task LoadFixedTranslations()
		{
			var preferedLanguage = await _profileProvider.GetLatestPreferedLanguage().ConfigureAwait(false);
			var language = preferedLanguage ?? DeviceLanguage;

			if(_fixedTranslations.Any())
			{
				return;
			}
            var fixedTranslations = await _fixedJsonFilesReader.GetFixedTranslations().ConfigureAwait(false);
			var translations = fixedTranslations.Where(x => x.Language == language);
			if (!translations.Any())
			{
				translations = fixedTranslations.Where(x => x.Language == DefaultLanguage);
			}
			_fixedTranslations = ToFiltredDictionary(translations);
		}
        
        private static Dictionary<TranslationKey, string> ToFiltredDictionary(IEnumerable<Translation> translations)
		{
			return translations.ToFiltredDictionary<Translation, TranslationKey, string>(x => x.Key, x => x.Value, x => x.Key.IsNotNull(), x => x.IsNotNull());
        }

		private static IEnumerable<KeyValuePair<TranslationKey, string>> ToKeyValuePairEnumerable(IEnumerable<Translation> translations)
		{
			foreach(var translation in translations)
			{
				yield return new KeyValuePair<TranslationKey, string>(translation.Key, translation.Value);
			}
		}

		public async Task SaveTranslations(IEnumerable<Translation> translations)
		{
			if(translations.Any())
			{
				await _unitOfWork.Translations.ReplaceAll(translations).ConfigureAwait(false);
				await LoadTranslations(true).ConfigureAwait(false);
				_mvxMessenger.Publish(new TranslationsLoadedMessage(this));
			}
		}

        public Task<IEnumerable<Translation>> GetTranslationsForKey(TranslationKey translationKey)
        {
            return _unitOfWork.Translations.GetAllAsync(x => x.Key == translationKey.Key);
        }

        public async Task ChangueLanguage(string language)
        {
            var profile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
            profile.PreferredLanguage = language;
            await _profileProvider.SaveUserProfile(profile).ConfigureAwait(false);
            await LoadTranslations(false).ConfigureAwait(false);
            _mvxMessenger.Publish(new TranslationsLoadedMessage(this));
        }
    }
}
