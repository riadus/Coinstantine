using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Coinstantine.Common.Attributes;
using Coinstantine.Data;
using Coinstantine.Domain.Extensions;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Mapping;
using Coinstantine.Domain.Mapping.DTOs;

namespace Coinstantine.Domain
{
    [RegisterInterfaceAsDynamic]
    public class FixedJsonFilesReader : IFixedJsonFilesReader
    {
        private IMapper<TranslationDTO, Translation> _translationMapper;
        public FixedJsonFilesReader(IMapperFactory mapperFactory)
        {
            _translationMapper = mapperFactory.GetMapper<TranslationDTO, Translation>();
        }

        public async Task<IEnumerable<Country>> GetCountries(string locale)
        {
            var countries = await ReadFixedJsonResource<Dictionary<string, string>>($"Countries.{locale.ToLower()}.country").ConfigureAwait(false);
            return countries.OrderBy(x => x.Value).Select(x => new Country { Langugage = x.Key, Name = x.Value });
        }

        public async Task<IEnumerable<Translation>> GetFixedTranslations()
        {
            var translations = await ReadFixedJsonResource<TranslationsDTO>("LocalTranslations").ConfigureAwait(false);
            if (translations.NumberOfTranslations > 0)
            {
                return translations.Translations.SelectMany(x => x.LanguageTranslations)
                                                .Select(x => _translationMapper.Map(x));
            }
            return new List<Translation>();
        }

        async Task<T> ReadFixedJsonResource<T>(string filePath)
        {
            var assembly = GetType().GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream($"Coinstantine.Domain.Files.{filePath}.json");
            string text;
            using (var reader = new StreamReader(stream))
            {
                text = await reader.ReadToEndAsync().ConfigureAwait(false);
            }
            return text.DeserializeTo<T>();
        }
    }
}