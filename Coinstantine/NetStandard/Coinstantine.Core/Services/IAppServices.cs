using Coinstantine.Core.UIServices;
using Coinstantine.Domain.Interfaces;
using MvvmCross.Navigation;
using Plugin.Connectivity.Abstractions;

namespace Coinstantine.Core.Services
{
	public interface IAppServices
	{
		INavigationService NavigationService { get; }
		ITranslationService TranslationService { get; }
		IMenuManager MenuManager { get; }
		IConnectivity Connectivity {get;}
		IOverlayService OverlayService { get; }
		IMessageService MessageService { get; }
        ISyncService SyncService { get; }
        IAnalyticsTracker AnalyticsTracker { get; }
        IStringFormatter StringFormatter { get; }
        IMvxNavigationService MvxNavigationService { get; }
    }
}
