using System.Threading.Tasks;

namespace Coinstantine.Core.Services
{
	public interface INotificationsAnalyserService
	{
		Task HandlePartToUpdate(string partToUpdate, bool isAppInForground);
		Task HandlePartToUpdateInForground();
	}
}
