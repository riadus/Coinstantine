namespace Coinstantine.Domain.Interfaces
{
    public interface IDeviceInfoProvider
    {
        string DeviceId { get; }
		string DeviceLanguage { get; }
    }
}
