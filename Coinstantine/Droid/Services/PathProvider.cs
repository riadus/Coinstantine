using System;
using System.IO;
using Coinstantine.Common.Attributes;
using Coinstantine.Domain.Interfaces;

namespace Coinstantine.Droid.Services
{
    [RegisterInterfaceAsDynamic]
    public class PathProvider : IPathProvider
    {
        private readonly string _personalFolder;

        public PathProvider()
        {
            _personalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        }

        public string DatabasePath => Path.Combine(_personalFolder, "Coinstantine.db");

        public string SecondDatabasePath => Path.Combine(_personalFolder, "Coinstantine2.db");

        public string ThirdDatabasePath => Path.Combine(_personalFolder, "Coinstantine3.db");

        public string CachePath => Path.Combine(_personalFolder, "Cache", "UserCache.txt");

        public string WhitePaperPath => Path.Combine(_personalFolder, "Documents", "WhitePaper.pdf");

        public string TermsAndServicesPath => Path.Combine(_personalFolder, "Documents", "TermsAndServices.pdf");

        public string PrivacyPolicyPath => Path.Combine(_personalFolder, "Documents", "PrivacyPolicy.pdf");

    }
}