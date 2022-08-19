using Coinstantine.Common;
using Coinstantine.Core.Services;
using Coinstantine.Domain.Interfaces.Translations;

namespace Coinstantine.Core.ViewModels.Generic
{
    public class GenericInfoItemViewModel : BaseViewModel
    {
        public GenericInfoItemViewModel(IAppServices appServices) : base(appServices)
        {
            Value3 = string.Empty;
        }

        public string Title1 => Translate(TranslatableTitle1);
        public string Title2 => TranslatableTitle2 != null ? Translate(TranslatableTitle2) : "";
        public bool IsTitle { get; set; }

        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }

        public bool ShowSecondTitle => Title2.IsNotNull() && ShowSecondValue; 
        public bool ShowSecondValue => Value2.IsNotNull();
        public bool ShowThirdValue => Value3.IsNotNull();

        public TranslationKey TranslatableTitle1 { get; set; }
        public TranslationKey TranslatableTitle2 { get; set; }
        public int Value1Lines => ShowSecondTitle || ShowThirdValue ? 1 : 2;
    }
}
