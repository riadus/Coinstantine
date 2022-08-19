using Coinstantine.Common;
using Coinstantine.Common.Attributes;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces;
using Foundation;

namespace Coinstantine.iOS.Services
{
    [RegisterInterfaceAsDynamic]
    public class AppEnvironmentProvider : IAppEnvironmentProvider
    {
        public AppEnvironmentProvider()
        {
            ClientId = NSBundle.MainBundle.ObjectForInfoDictionary("ClientId")?.ToString();
            Secret = NSBundle.MainBundle.ObjectForInfoDictionary("Secret")?.ToString();
            NotificationHub = NSBundle.MainBundle.ObjectForInfoDictionary("NotificationHub")?.ToString();
            NotificationConnectionString = NSBundle.MainBundle.ObjectForInfoDictionary("NotificationConnectionString")?.ToString();
            var apiEnvironment = NSBundle.MainBundle.ObjectForInfoDictionary("ApiEnvironment")?.ToString();
            if (apiEnvironment.IsNotNull())
            {
                ApiEnvironment = apiEnvironment.ToEnum<ApiEnvironment>();
            }
        }

        public ApiEnvironment ApiEnvironment { get; }
        public string ClientId { get; }
        public string Secret { get; }
        public string NotificationHub { get; }
        public string NotificationConnectionString { get; }
    }
}
