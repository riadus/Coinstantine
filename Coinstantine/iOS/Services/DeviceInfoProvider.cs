using System;
using System.Linq;
using Foundation;
using Coinstantine.Common.Attributes;
using Coinstantine.Domain.Interfaces;

namespace Coinstantine.iOS.Services
{
	[RegisterInterfaceAsDynamic]
    public class DeviceInfoProvider : IDeviceInfoProvider
    {
        public string DeviceId => throw new NotImplementedException();

		public string DeviceLanguage => NSLocale.PreferredLanguages.Any() ? NSLocale.PreferredLanguages[0] : "en-US";
	}
}
