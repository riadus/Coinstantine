using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces.Translations;

namespace Coinstantine.Domain.Interfaces
{
    public interface ITranslationService
    {
		string Translate(TranslationKey key, string defaultValue);
		string Translate(TranslationKey key);
		Task SaveTranslations(IEnumerable<Translation> translations);
		string DeviceLanguage { get; set; }
		string CurrentLanguage { get; }
        CultureInfo CurrentCulture { get; }
		Task LoadTranslations(bool forceLoadDefault);
        Task<IEnumerable<Translation>> GetTranslationsForKey(TranslationKey translationKey);
        Task ChangueLanguage(string language);
    }

    public interface IStringFormatter
    {
        string FormatDateM(DateTime dateTime);
        string FormatDateD(DateTime dateTime);
        string FormatDateF(DateTime dateTime);
        string FormatDateT(DateTime dateTime);
    }
}
