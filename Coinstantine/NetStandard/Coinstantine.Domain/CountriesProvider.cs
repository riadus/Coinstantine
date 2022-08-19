using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coinstantine.Common.Attributes;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces;

namespace Coinstantine.Domain
{
    [RegisterInterfaceAsDynamic]
    public class CountriesProvider : ICountriesProvider
    {
        private readonly IFixedJsonFilesReader _fixedJsonFilesReader;
        private readonly ITranslationService _translationService;
        private readonly Dictionary<char, string> _letters;

        public CountriesProvider(IFixedJsonFilesReader fixedJsonFilesReader, ITranslationService translationService)
        {
            _fixedJsonFilesReader = fixedJsonFilesReader;
            _translationService = translationService;
            _letters = new Dictionary<char, string>
            {
                {'A', char.ConvertFromUtf32(0x1F1E6)},
                {'B', char.ConvertFromUtf32(0x1F1E7)},
                {'C', char.ConvertFromUtf32(0x1F1E8)},
                {'D', char.ConvertFromUtf32(0x1F1E9)},
                {'E', char.ConvertFromUtf32(0x1F1EA)},
                {'F', char.ConvertFromUtf32(0x1F1EB)},
                {'G', char.ConvertFromUtf32(0x1F1EC)},
                {'H', char.ConvertFromUtf32(0x1F1ED)},
                {'I', char.ConvertFromUtf32(0x1F1EE)},
                {'J', char.ConvertFromUtf32(0x1F1EF)},
                {'K', char.ConvertFromUtf32(0x1F1F0)},
                {'L', char.ConvertFromUtf32(0x1F1F1)},
                {'M', char.ConvertFromUtf32(0x1F1F2)},
                {'N', char.ConvertFromUtf32(0x1F1F3)},
                {'O', char.ConvertFromUtf32(0x1F1F4)},
                {'P', char.ConvertFromUtf32(0x1F1F5)},
                {'Q', char.ConvertFromUtf32(0x1F1F6)},
                {'R', char.ConvertFromUtf32(0x1F1F7)},
                {'S', char.ConvertFromUtf32(0x1F1F8)},
                {'T', char.ConvertFromUtf32(0x1F1F9)},
                {'U', char.ConvertFromUtf32(0x1F1FA)},
                {'V', char.ConvertFromUtf32(0x1F1FB)},
                {'W', char.ConvertFromUtf32(0x1F1FC)},
                {'X', char.ConvertFromUtf32(0x1F1FD)},
                {'Y', char.ConvertFromUtf32(0x1F1FE)},
                {'Z', char.ConvertFromUtf32(0x1F1FF)}
            };
        }

        public async Task<IEnumerable<Country>> GetCountries()
        {
            var countries = await _fixedJsonFilesReader.GetCountries(_translationService.CurrentLanguage).ConfigureAwait(false);
            return countries.Select(x =>
            {
                x.Flag = GetFlag(x.Langugage);
                return x;
            });
        }

        private string GetFlag(string country)
        {
            var l1 = country[0];
            var l2 = country[1];
            return _letters[l1] + _letters[l2];
        }
    }
}
