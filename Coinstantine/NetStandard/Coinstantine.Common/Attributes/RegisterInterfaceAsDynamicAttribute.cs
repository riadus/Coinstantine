using System;
namespace Coinstantine.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RegisterInterfaceAsDynamicAttribute : RegisterInterfaceAttribute
    {
        public RegisterInterfaceAsDynamicAttribute() : base()
        {

        }

        public RegisterInterfaceAsDynamicAttribute(IoCPlatform ioCPlatform) : base(ioCPlatform)
        {
        }
    }

    public enum IoCPlatform
    {
        iOS,
        Android,
        All,
        None
    }
}
