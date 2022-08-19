using System.Diagnostics;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Coinstantine.Common.Attributes;
using Coinstantine.Core;
using Coinstantine.Data;
using Coinstantine.Database;
using Coinstantine.Domain;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Droid.CustomBindings;
using Coinstantine.Droid.CustomViews;
using Coinstantine.Droid.CustomViews.IconViews;
using Coinstantine.Droid.Services;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.ViewModels;
using Plugin.Connectivity;
using Plugin.Fingerprint;
using Xamarin.Auth;

namespace Coinstantine.Droid
{
    public class Setup : MvxAndroidSetup<App>
    {
        protected override IMvxApplication CreateApp()
        {
            SetupDatabase();
            return new App();
        }

       private void SetupDatabase()
        {
            var pathProvider = Mvx.Resolve<IPathProvider>();
            var path = pathProvider.DatabasePath;
            var secondPath = pathProvider.SecondDatabasePath;
            var thirdPath = pathProvider.ThirdDatabasePath;

            SqliteDatabase.SetFilePath(SqliteDatabase.ConnectionType.First, path);
            SqliteDatabase.SetFilePath(SqliteDatabase.ConnectionType.Second, secondPath);
            SqliteDatabase.SetFilePath(SqliteDatabase.ConnectionType.Third, thirdPath);

            Debug.WriteLine($"Database is located at {path}");

            SqliteDatabase.Initialize();
        }

        protected override void InitializePlatformServices()
        {
            Mvx.RegisterSingleton(AccountStore.Create("password"));
            Mvx.RegisterType(() => CrossFingerprint.Current);
            CrossFingerprint.SetDialogFragmentType<AppFingerprintDialogFragment>();
            RegisterPlugins();
            RegisterServicesFromAllAssemblies();
            base.InitializePlatformServices();
        }

        protected override void InitializeLifetimeMonitor()
        {
            base.InitializeLifetimeMonitor();
            var androidLifeCycle = Mvx.Resolve<IAndroidLifeCycle>();
            androidLifeCycle.Initialize();
        }

        private void RegisterPlugins()
        {
            Mvx.RegisterType(() => CrossConnectivity.Current);
        }

        private void RegisterServicesFromAllAssemblies()
        {
            new DomainAssemblyReference().RegisterServicesForAssembly(IoCPlatform.Android);
            new DatabaseAssemblyReference().RegisterServicesForAssembly(IoCPlatform.Android);
            new DataAssemblyReference().RegisterServicesForAssembly(IoCPlatform.Android);
            new DroidAssemblyReference().RegisterServicesForAssembly(IoCPlatform.Android);
            new CoreAssemblyReference().RegisterServicesForAssembly(IoCPlatform.Android);
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);
            registry.RegisterCustomBindingFactory<TextView>("AppString", textView => new AppFontLabelBinding(textView));
            registry.RegisterCustomBindingFactory<IconTextView>("AppString", textView => new IconTextViewBinding(textView));
            registry.RegisterCustomBindingFactory<Button>("AppString", button => new AppFontButtonBinding(button));
            registry.RegisterCustomBindingFactory<TextView>("TextColor", textview => new TextViewTextColorBinding(textview));
            registry.RegisterCustomBindingFactory<Button>("TextColor", button => new ButtonTextColorBinding(button));
            registry.RegisterCustomBindingFactory<IconEditText>("Icon", iconEditText => new IconEditTextBinding(iconEditText));
            registry.RegisterCustomBindingFactory<View>("Pin", view => new PinBackgroundBinding(view));
            registry.RegisterCustomBindingFactory<TextView>("Disabled", textView => new DisabledTextViewBindings(textView));
            registry.RegisterCustomBindingFactory<Button>("CSN", button => new BuyCoinstantineButtonBinding(button));
            registry.RegisterCustomBindingFactory<TextView>("Lines", textView => new TextViewLinesBinding(textView));
            registry.RegisterCustomBindingFactory<TextView>("TextSize", textView => new TextViewTextSizeBinding(textView));
            registry.RegisterCustomBindingFactory<WebView>("JustifiedText", webView => new JustifiedTextBinding(webView)); 
            registry.RegisterCustomBindingFactory<Button>("Enabled", button => new DisabledButtonBindings(button));
            registry.RegisterPropertyInfoBindingFactory(typeof(IconEditTextGrossValueBinding), typeof(IconEditText), "GrossHiddenValue");
        }
    }

    public class DroidAssemblyReference : AssemblyReference
    {

    }
}
