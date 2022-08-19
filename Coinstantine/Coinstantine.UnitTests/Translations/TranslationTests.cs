using System;
using System.Linq;
using Coinstantine.Domain.Interfaces.Translations;
using NUnit.Framework;

namespace Coinstantine.UnitTests.Translations
{
    public class TranslationTests
    {
        [Test]
        public void French_And_English_Translations_Must_Have_The_Same_Number_Of_Occurence()
        {
            var translationsEn = new TranslationBuilder()
                .WithFileName("Translations-en")
                .WithSkip(1)
                .WithSeparator(';')
                .Build();

            var translationsFr = new TranslationBuilder()
                .WithFileName("Translations-fr")
                .WithSkip(1)
                .WithSeparator(';')
                .Build();

            Assert.AreEqual(translationsEn.Count(), translationsFr.Count());
        }

        [Test]
        public void All_Translation_Keys_In_File_Must_Be_In_App()
        {
            var translationsEn = new TranslationBuilder()
                .WithFileName("Translations-en")
                .WithSkip(1)
                .WithSeparator(';')
                .Build();

            var translationKeys = new TranslationKeysRetreiver().GetKeysFromType(typeof(TranslationKeys));
            var appTranslations = translationKeys.SelectMany(x => x.Value).ToList();
            foreach (var translation in translationsEn)
            {
                Assert.Contains(translation.Key, appTranslations);
            }
        }

        [Test]
        public void All_Translation_Keys_In_App_Must_Be_In_File()
        {
            var translationsEn = new TranslationBuilder()
                .WithFileName("Translations-en")
                .WithSkip(1)
                .WithSeparator(';')
                .Build();

            var translationKeys = new TranslationKeysRetreiver()
                .SkipKeyStartingWith("Fixed")
                .SkipKeyStartingWith("White_space")
                .GetKeysFromType(typeof(TranslationKeys));

            var fileTranslations = translationsEn.Select(x => x.Key).ToList();
            foreach (var translation in translationKeys.SelectMany(x => x.Value))
            {
                Assert.Contains(translation, fileTranslations);
            }
        }

        [Test]
        public void Number_Of_Translations_In_App_Must_Match_Translation_File()
        {
            var translationsEn = new TranslationBuilder()
                .WithFileName("Translations-en")
                .WithSkip(1)
                .WithSeparator(';')
                .Build();

            var translationKeys = new TranslationKeysRetreiver()
                .SkipKeyStartingWith("Fixed")
                .SkipKeyStartingWith("White_space")
                .GetKeysFromType(typeof(TranslationKeys));

            var appTranslationsCount = translationKeys.SelectMany(x => x.Value).Count();

            Assert.AreEqual(translationsEn.Count(), appTranslationsCount);
        }

        [Test]
        public void Get_Keys_In_App_And_Not_In_File()
        {
            var translationsEn = new TranslationBuilder()
                .WithFileName("Translations-fr")
                .WithSkip(1)
                .WithSeparator(';')
                .Build();

            var translationKeys = new TranslationKeysRetreiver()
                .SkipKeyStartingWith("Fixed")
                .SkipKeyStartingWith("White_space")
                .GetKeysFromType(typeof(TranslationKeys));

            var numberOfMissingKeys = 0;
            foreach (var field in translationKeys)
            {
                var missingKeys = field.Value.Except(translationsEn.Select(x => x.Key));
                if (missingKeys.Any())
                {
                    Console.WriteLine($"* -- Nested class : {field.Key}");
                    missingKeys.ToList().ForEach(x => Console.WriteLine($"  * - {x}"));
                    numberOfMissingKeys++;
                }
            }

            Assert.AreEqual(0, numberOfMissingKeys);
        }

        [Test]
        public void Get_Keys_In_File_And_Not_In_App()
        {
            var translationsEn = new TranslationBuilder()
                .WithFileName("Translations-en")
                .WithSkip(1)
                .WithSeparator(';')
                .Build();

            var translationKeys = new TranslationKeysRetreiver()
                .SkipKeyStartingWith("Fixed")
                .SkipKeyStartingWith("White_space")
                .GetKeysFromType(typeof(TranslationKeys));

            var keyInApp = translationKeys.SelectMany(x => x.Value);
            var keysInFile = translationsEn.Select(x => x.Key);
            var appCount = keyInApp.Count();
            var fileCount = keysInFile.Count();
            var dAppCount = keyInApp.Distinct().Count();
            var dfileCount = keysInFile.Distinct().Count();
            var missingKeys = keyInApp.Except(keysInFile);

            missingKeys.ToList().ForEach(x => Console.WriteLine($"Missing Key In App - {x}"));

            Assert.AreEqual(0, missingKeys.Count());

        }

        [Test]
        public void File_Must_Not_Have_Duplicates()
        {
            var translationsEn = new TranslationBuilder()
                .WithFileName("Translations-en")
                .WithSkip(1)
                .WithSeparator(';')
                .Build();

            var duplicates = translationsEn.GroupBy(x => x)
                        .Where(group => group.Count() > 1)
                        .Select(group => group.Key);

            Assert.AreEqual(0, duplicates.Count());
        }

        [Test]
        public void App_Must_Not_Have_Duplicates()
        {
            var translationKeys = new TranslationKeysRetreiver()
                .GetKeysFromType(typeof(TranslationKeys));

            var duplicates = translationKeys.SelectMany(x => x.Value).GroupBy(x => x)
                        .Where(group => group.Count() > 1)
                        .Select(group => group.Key);

            Assert.AreEqual(0, duplicates.Count());
        }
    }
}
