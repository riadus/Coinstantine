using Coinstantine.Domain;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Auth;
using MvvmCross.ViewModels;
using MvvmCross.IoC;
using MvvmCross;

namespace Coinstantine.Core
{
    public class App : MvxApplication
    {
        public App()
        {
            RegisterAllViewModels();
            RegisterServices();
        }

        public override void Initialize()
        {
            RegisterCustomAppStart<AppStart>();
        }

        private void RegisterAllViewModels()
        {
            CreatableTypes().EndingWith("ViewModel").AsTypes().RegisterAsDynamic();
        }

        private void RegisterServices()
        {
            Mvx.RegisterType<IApiClient>(() => new AuthApiClient(
                Mvx.IoCConstruct<ApiClient>(),
                Mvx.Resolve<ITokenRefreshService>(),
                Mvx.Resolve<IAnalyticsTracker>(),
                Mvx.Resolve<ITokenProvider>(),
                Mvx.Resolve<ILogoutService>(),
                Mvx.Resolve<IEndpointProvider>()));
        }
    }
}
