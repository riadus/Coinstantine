using System.Collections.Generic;

namespace Coinstantine.Domain.Mapping.DTOs
{
    public class LanguageTranslationsDTO
    {
        public int NumberOfTranslations { get; set; }
        public string Language { get; set; }
        public List<TranslationDTO> LanguageTranslations { get; set; }
    }
}
