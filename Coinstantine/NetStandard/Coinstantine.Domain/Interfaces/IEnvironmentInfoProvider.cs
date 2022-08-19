using System.Threading.Tasks;
using Coinstantine.Data;

namespace Coinstantine.Domain.Interfaces
{
    public interface IEnvironmentInfoProvider
    {
        Task<string> GetValue(SettingKey settingKey);
        Task<string> GetWeb3Url();
        Task<string> GetEtherscanUrl();
        Task SetUrlsFromBackend();
    }

    public interface IAppEnvironmentProvider
    {
        ApiEnvironment ApiEnvironment { get; }
        string ClientId { get; }
        string Secret { get; }
        string NotificationHub { get; }
        string NotificationConnectionString { get; }
    }
}
