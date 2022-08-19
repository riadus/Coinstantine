using Coinstantine.Common.Attributes;
using Coinstantine.Domain.Interfaces;

namespace Coinstantine.Domain
{
    [RegisterInterfaceAsDynamic]
    public class TelegramBotProvider : ITelegramBotProvider
    {
        private readonly IAppEnvironmentProvider _appEnvironmentProvider;

        public TelegramBotProvider(IAppEnvironmentProvider appEnvironmentProvider)
        {
            _appEnvironmentProvider = appEnvironmentProvider;
        }

        public string GetBotLink()
        {
            switch(_appEnvironmentProvider.ApiEnvironment)
            {
                case Data.ApiEnvironment.Acceptance:
                case Data.ApiEnvironment.Localhost:
                    return "MassDistribot";
                case Data.ApiEnvironment.Preprod:
                    return "CoinstantinePreprodBot";
                case Data.ApiEnvironment.Production:
                    return "CoinstantineBot";
            }
            return string.Empty;
        }
    }
}
