using Coinstantine.Common;
using Coinstantine.Common.Attributes;
using Coinstantine.Domain.Interfaces;
using Java.Util;

namespace Coinstantine.Droid.Services
{
    [RegisterInterfaceAsDynamic]
    public class DeviceInfoProvider : IDeviceInfoProvider
    {
        public string DeviceId => throw new System.NotImplementedException();

        public string DeviceLanguage => GetDeviceLanguage();

        private string GetDeviceLanguage()
        {
            return (Locale.Default?.Language.IsNotNull() ?? false) && (Locale.Default?.Country.IsNotNull() ?? false)
                ? $"{Locale.Default.Language}-{Locale.Default.Country}"
                : "en-US";
        }
    }
}