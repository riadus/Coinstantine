using Coinstantine.Common.Attributes;
using Coinstantine.Domain.Interfaces;

namespace Coinstantine.Domain
{
    [RegisterInterfaceAsDynamic]
    public class EndpointProvider : IEndpointProvider
    {
        public EndpointProvider(IAppEnvironmentProvider appEnvironmentProvider)
        {
            switch (appEnvironmentProvider.ApiEnvironment)
            {
                case Data.ApiEnvironment.Acceptance:
                    Endpoint = "https://coinstantine-acceptance.azurewebsites.net/api/";
                    WebsiteEndpoint = "https://app-acc.coinstantine.io/";
                    break;
                case Data.ApiEnvironment.Preprod:
                    Endpoint = "https://coinstantine-preprod.azurewebsites.net/api/";
                    WebsiteEndpoint = "https://app-acc.coinstantine.io/";
                    break;
                case Data.ApiEnvironment.Production:
                    Endpoint = "https://coinstantine.azurewebsites.net/api/";
                    WebsiteEndpoint = "https://app.coinstantine.io/";
                    break;
                case Data.ApiEnvironment.Localhost:
                default:
                    Endpoint = "http://localhost:5000/api/";
                    WebsiteEndpoint = "http://localhost:4200/";
                    break;
            }
            ClientId = appEnvironmentProvider.ClientId;
            Secret = appEnvironmentProvider.Secret;
        }

        public string Endpoint { get; }
        public string ClientId { get; }
        public string Secret { get; }
        public string WebsiteEndpoint { get; }
    }
}
