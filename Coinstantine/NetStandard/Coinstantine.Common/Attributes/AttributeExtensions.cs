using System.Reflection;
using System.Collections.Generic;
using System;

namespace Coinstantine.Common.Attributes
{
    public static class AttributeExtensions
    {
        public static IEnumerable<Type> WithAttributeProperty<TAttribute>(this IEnumerable<Type> types, Func<TAttribute, bool> func) where TAttribute : Attribute
        {
            foreach (var type in types)
            {
                var attribute = type.GetCustomAttribute(typeof(TAttribute), true);
                if (attribute != null && func((TAttribute)attribute))
                {
                    yield return type;
                }
            }
        }

        public static IEnumerable<Type> WithPlatformAttribute<TAttribute>(this IEnumerable<Type> types, IoCPlatform platform) where TAttribute : RegisterInterfaceAttribute
        {
            return WithAttributeProperty<TAttribute>(types, t => t.IoCPlatform == platform || t.IoCPlatform == IoCPlatform.All);
        }
    }
}
