using System;

namespace Coinstantine.Common
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string self) => string.IsNullOrEmpty(self) || string.IsNullOrWhiteSpace(self);
        public static bool IsNotNull(this string self) => !self.IsNullOrEmpty();
        public static string AddReturnLine(this string self)
        {
            if(self.IsNotNull())
            {
                return self + "\n";
            }
            return string.Empty;
        }

        public static object ToEnum(this string val, Type targetType)
        {
            if (val.IsNullOrEmpty())
            {
                return null;
            }

            var nullableType = Nullable.GetUnderlyingType(targetType);

            if (nullableType != null)
            {
                targetType = nullableType;
            }

            var value = val.Replace("-", "").Replace(" ", "");
            var enumValue = Enum.Parse(targetType, value, true);
            return enumValue;
        }

        public static T ToEnum<T>(this string val) => val.IsNullOrEmpty() ? default(T) : (T)ToEnum(val, typeof(T));

        public static string Sanitize(this string self)
        {
            var result = self.TrimEnd();
            if (result.StartsWith("@", StringComparison.OrdinalIgnoreCase))
            {
                result = result.Remove(0, 1);
            }
            return result;
        }
    }
}
