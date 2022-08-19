using Coinstantine.Common.Attributes;
using Coinstantine.Core.Services;
using Coinstantine.Domain.Interfaces;
using Foundation;

namespace Coinstantine.iOS.Services
{
    [RegisterInterfaceAsLazySingleton]
    public class AppVersionIOS : AppVersion
    {
        public AppVersionIOS(IEnvironmentInfoProvider environmentInfoProvider) : base(environmentInfoProvider)
        {
            LoadVersions();
        }

        private void LoadVersions()
        {
            Version = NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleShortVersionString")?.ToString();
            BuildVersion = NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleVersion")?.ToString();
        }

        public override string Version { get; protected set; }

        public override string BuildVersion { get; protected set; }
    }
}