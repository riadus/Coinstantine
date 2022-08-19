using Coinstantine.Common.Attributes;
using Coinstantine.Core.UIServices;
using Coinstantine.Domain.Interfaces;
using MvvmCross.Navigation;
using Plugin.Connectivity.Abstractions;

namespace Coinstantine.Core.Services
{
    [RegisterInterfaceAsLazySingleton]
    public class AppServices : IAppServices
    {
        public AppServices(INavigationService navigationService,
                           IMvxNavigationService mvxNavigationService,
                           ITranslationService translationService,
                           IMenuManager menuManager,
                           IConnectivity connectivity,
                           IOverlayService overlayService,
                           IMessageService messageService,
                           ISyncService syncService,
                           IAnalyticsTracker analyticsTracker,
                           IStringFormatter stringFormatter)
        {
            NavigationService = navigationService;
            MvxNavigationService = mvxNavigationService;
            TranslationService = translationService;
            MenuManager = menuManager;
            Connectivity = connectivity;
            OverlayService = overlayService;
            MessageService = messageService;
            SyncService = syncService;
            AnalyticsTracker = analyticsTracker;
            StringFormatter = stringFormatter;
        }

        public INavigationService NavigationService { get; }

        public ITranslationService TranslationService { get; }

        public IMenuManager MenuManager { get; }

        public IConnectivity Connectivity { get; }

        public IOverlayService OverlayService { get; }

        public IMessageService MessageService { get; }

        public ISyncService SyncService { get; }

        public IAnalyticsTracker AnalyticsTracker { get; }

        public IStringFormatter StringFormatter { get; }

        public IMvxNavigationService MvxNavigationService { get; }
    }
}