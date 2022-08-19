using Android.Content;
using Coinstantine.Common.Attributes;
using Coinstantine.Core.Services;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Droid.Helpers;

namespace Coinstantine.Droid.Services
{
    [RegisterInterfaceAsDynamic]
    public class AppVersionDroid : AppVersion
    {
        private readonly Context _context;
        public AppVersionDroid(IEnvironmentInfoProvider environmentInfoProvider) : base(environmentInfoProvider)
        {
            _context = Android.App.Application.Context;
            LoadVersion();
        }

        private void LoadVersion()
        {
            Version = _context.PackageManager.GetPackageInfo(_context.PackageName, 0).VersionName;
            BuildVersion = AppParameters.GetAppParameter("BuildVersion");
        }

        public override string Version { get; protected set; }
        public override string BuildVersion { get; protected set; }
    }
}