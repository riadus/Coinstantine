using System.Diagnostics;
using Coinstantine.Common.Attributes;
using Coinstantine.Core;
using Coinstantine.Data;
using Coinstantine.Database;
using Coinstantine.Domain;
using Coinstantine.Domain.Interfaces;
using Coinstantine.iOS.CustomBindings;
using Foundation;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Platforms.Ios.Core;
using MvvmCross.ViewModels;
using Plugin.Connectivity;
using UIKit;
using Xamarin.Auth;

namespace Coinstantine.iOS
{
    public class Setup<T> : MvxIosSetup<T> where T : class, IMvxApplication, new()
    {
        protected override IMvxApplication CreateApp()
        {
            SetupDatabase();
            return new App();
        }

        protected override void RegisterPlatformProperties()
        {
            Mvx.RegisterSingleton(AccountStore.Create);
            RegisterPlugins();
            RegisterServicesFromAllAssemblies();
            base.RegisterPlatformProperties();
        }

        private void RegisterPlugins()
        {
            Mvx.RegisterType(() => CrossConnectivity.Current);
        }

        private void RegisterServicesFromAllAssemblies()
        {
            new DomainAssemblyReference().RegisterServicesForAssembly(IoCPlatform.iOS);
            new DatabaseAssemblyReference().RegisterServicesForAssembly(IoCPlatform.iOS);
            new DataAssemblyReference().RegisterServicesForAssembly(IoCPlatform.iOS);
            new IosAssemblyReference().RegisterServicesForAssembly(IoCPlatform.iOS);
            new CoreAssemblyReference().RegisterServicesForAssembly(IoCPlatform.iOS);
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

            //          string password = GetDatabaseEncryptionKey();
            //#if DEBUG
            //          // we want to be sure that this never ends up in Release builds, 
            //          // but we need it when we want to diagnose the database during bugfixing
            //          Debug.WriteLine($"Using encryption key {password}");
            //#endif
            //          var p = new SQLite.Net.Platform.SQLCipher.XamarinIOS.SQLitePlatformIOS(password);

            SqliteDatabase.Initialize();
            NSFileManager.SetSkipBackupAttribute(path, true);
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);
            registry.RegisterCustomBindingFactory<UIButton>("TitleColor", button => new TextColorButtonBinding(button));
            registry.RegisterCustomBindingFactory<UILabel>("AppString", label => new AppFontLabelBinding(label));
            registry.RegisterCustomBindingFactory<UIButton>("AppString", button => new AppFontButtonBinding(button));
            registry.RegisterCustomBindingFactory<IconTextField>("AppString", textField => new AppFontCircleBinding(textField));
            registry.RegisterCustomBindingFactory<CircleAppStringLabel>("AppString", circleLabel => new CircleAppStringLabelBinding(circleLabel));
            registry.RegisterCustomBindingFactory<UILabel>("AppStringReversed", label => new CircleAppStringLabelReversedBinding(label));
            registry.RegisterCustomBindingFactory<UIButton>("CSN", button => new BuyCoinstantineButtonBinding(button));
            registry.RegisterCustomBindingFactory<UILabel>("FontSize", label => new FontSizeLabelBinding(label));
            registry.RegisterPropertyInfoBindingFactory(typeof(IconTextfieldBinding), typeof(IconTextField), "GrossHiddenValue");
        }
    }

    public class IosAssemblyReference : AssemblyReference
    {
        
    }
}
