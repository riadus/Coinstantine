using System;
namespace Coinstantine.Common.Attributes
{
    public abstract class RegisterInterfaceAttribute : Attribute
    {
        public IoCPlatform IoCPlatform { get; }
        protected RegisterInterfaceAttribute(IoCPlatform ioCPlatform)
        {
            IoCPlatform = ioCPlatform;
        }

        protected RegisterInterfaceAttribute() : this(IoCPlatform.All)
        {

        }
    }
}
