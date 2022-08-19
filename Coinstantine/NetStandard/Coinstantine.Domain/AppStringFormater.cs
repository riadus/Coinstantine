using System;
using Coinstantine.Common.Attributes;
using Coinstantine.Domain.Interfaces;

namespace Coinstantine.Domain
{
    [RegisterInterfaceAsDynamic]
    public class AppStringFormater : IStringFormatter
    {
        private readonly ITranslationService _translationService;

        public AppStringFormater(ITranslationService translationService)
        {
            _translationService = translationService;
        }

        public string FormatDateM(DateTime dateTime)
        {
            return dateTime.ToString("M", _translationService.CurrentCulture);
        }

        public string FormatDateF(DateTime dateTime)
        {
            return dateTime.ToString("F", _translationService.CurrentCulture);
        }

        public string FormatDateD(DateTime dateTime)
        {
            return dateTime.ToString("D", _translationService.CurrentCulture);
        }

        public string FormatDateT(DateTime dateTime)
        {
            return dateTime.ToString("t", _translationService.CurrentCulture);
        }
    }
}
