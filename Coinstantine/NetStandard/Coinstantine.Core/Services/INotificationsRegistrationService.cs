using System.Threading.Tasks;

namespace Coinstantine.Core.Services
{
    public interface INotificationsRegistrationService
    {
        Task RegisterForNotifications(string email);
        Task<bool> AreNotificationsEnabled();
        Task Unregister();
    }
}