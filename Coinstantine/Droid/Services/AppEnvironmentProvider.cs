using Coinstantine.Common.Attributes;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Droid.Helpers;
using Coinstantine.Common;

namespace Coinstantine.Droid.Services
{
    [RegisterInterfaceAsDynamic]
    public class AppEnvironmentProvider : IAppEnvironmentProvider
    {
        public ApiEnvironment ApiEnvironment => AppParameters.GetAppParameter("ApiEnvironment").ToEnum<ApiEnvironment>();

        public string ClientId => AppParameters.GetAppParameter("ClientId");

        public string Secret => AppParameters.GetAppParameter("Secret");

        public string NotificationHub => AppParameters.GetAppParameter("NotificationHub");

        public string NotificationConnectionString => AppParameters.GetAppParameter("NotificationConnectionString");
    }
}