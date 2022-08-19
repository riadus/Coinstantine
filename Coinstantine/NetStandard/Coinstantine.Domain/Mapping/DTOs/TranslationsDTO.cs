using System.Collections.Generic;

namespace Coinstantine.Domain.Mapping.DTOs
{
    public class TranslationsDTO
    {
        public int NumberOfTranslations { get; set; }
        public List<string> SupportedLanguages { get; set; }
        public List<LanguageTranslationsDTO> Translations { get; set; }
    }
}
