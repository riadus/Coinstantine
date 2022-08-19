using System;
namespace Coinstantine.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RegisterInterfaceAsLazySingletonAttribute : RegisterInterfaceAttribute
    {
        public RegisterInterfaceAsLazySingletonAttribute() : base()
        {

        }

        public RegisterInterfaceAsLazySingletonAttribute(IoCPlatform ioCPlatform) : base(ioCPlatform)
        {
        }
    }
}
