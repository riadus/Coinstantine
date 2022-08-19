using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Coinstantine.Domain.Interfaces.Translations;

namespace Coinstantine.UnitTests.Translations
{
    public class TranslationBuilder
    {
        private string _filePath;
        private string _fileName;
        private char _separator = ';';
        private int _skip = 1;
        public IEnumerable<Translation> Build()
        {
            if (_filePath != null)
            {
                return File.ReadAllLines(_filePath)
                                .Skip(_skip)
                                .Select(v => Translation.FromCsv(v, _separator))
                                .ToList();
            }
            return ReadFixedCsvResource(_fileName, _separator, _skip);
        }

        public TranslationBuilder WithFilePath(string filePath)
        {
            _filePath = filePath;
            return this;
        }

        public TranslationBuilder WithFileName(string fileName)
        {
            _fileName = fileName;
            return this;
        }

        public TranslationBuilder WithSeparator(char separator)
        {
            _separator = separator;
            return this;
        }

        public TranslationBuilder WithSkip(int skip)
        {
            _skip = skip;
            return this;
        }

        private static IEnumerable<Translation> ReadFixedCsvResource(string fileName, char separator, int skip)
        {
            var assembly = typeof(TranslationBuilder).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream($"Coinstantine.UnitTests.Translations.{fileName}.csv");
            string text;
            using (var reader = new StreamReader(stream))
            {
                var i = 0;
                while ((text = reader.ReadLine()) != null)
                {
                    if (i >= skip)
                    {
                        yield return Translation.FromCsv(text, separator);
                    }
                    i++;
                }
            }
        }
    }

    public class Translation
    {
        public TranslationKey Key { get; set; }
        public string Value { get; set; }

        public static Translation FromCsv(string csvLine, char separateor)
        {
            string[] values = csvLine.Split(separateor);
            Translation translation = new Translation
            {
                Key = values[0],
                Value = values[1]
            };
            return translation;
        }
    }
}
