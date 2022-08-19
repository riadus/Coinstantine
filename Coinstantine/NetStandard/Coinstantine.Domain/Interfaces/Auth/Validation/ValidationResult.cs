using Coinstantine.Domain.Interfaces.Translations;

namespace Coinstantine.Domain.Interfaces.Auth.Validation
{
    public class ValidationResult<TReturnType>
    {
        public TReturnType Result { get; set; }
        public TranslationKey ErrorMessage { get; set; }
    }
}
