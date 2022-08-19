using System.Threading.Tasks;
using Coinstantine.Common;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces;

namespace Coinstantine.Core.Services
{
    public interface IAppVersion
    {
        string Version { get; }
        string BuildVersion { get; }
        Task<EthereumNetwork> GetEthereumNetwork();
        Task<ApiEnvironment> GetApiEnvironment();
    }

    public abstract class AppVersion : IAppVersion
    {
        private readonly IEnvironmentInfoProvider _environmentInfoProvider;

        protected AppVersion(IEnvironmentInfoProvider environmentInfoProvider)
        {
            _environmentInfoProvider = environmentInfoProvider;
        }

        public abstract string Version { get; protected set; }

        public abstract string BuildVersion { get; protected set;}

        public async Task<ApiEnvironment> GetApiEnvironment()
        {
            var apiEnvironment = await _environmentInfoProvider.GetValue(SettingKey.ApiEnvironment).ConfigureAwait(false);
            return apiEnvironment.ToEnum<ApiEnvironment>();
        }

        public async Task<EthereumNetwork> GetEthereumNetwork()
        {
            var ethereumNetwork = await _environmentInfoProvider.GetValue(SettingKey.EthereumNetwork).ConfigureAwait(false);
            return ethereumNetwork.ToEnum<EthereumNetwork>();
        }
    }
}
