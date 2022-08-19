using System.Reflection;
using System.Linq;
using MvvmCross.IoC;

namespace Coinstantine.Common.Attributes
{
    public abstract class AssemblyReference
    {
        public void RegisterServicesForAssembly(IoCPlatform platform)
        {
            var assembly = GetType().GetTypeInfo().Assembly;
            var creatableTypes = assembly.CreatableTypes().ToList();
            creatableTypes.WithPlatformAttribute<RegisterInterfaceAsDynamicAttribute>(platform).AsInterfaces().RegisterAsDynamic(); ;
            creatableTypes.WithPlatformAttribute<RegisterInterfaceAsLazySingletonAttribute>(platform).AsInterfaces().RegisterAsLazySingleton();
        }
    }
}