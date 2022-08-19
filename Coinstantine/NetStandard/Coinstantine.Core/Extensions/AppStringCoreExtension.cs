using Coinstantine.Core.Fonts;
using MvvmCross;

namespace Coinstantine.Core.Extensions
{
    public static class AppStringCoreExtension
    {
        private static ICodeFontProvider _codeFontProvider;
        private static ICodeFontProvider CodeFontProvider => _codeFontProvider ?? (_codeFontProvider = Mvx.Resolve<ICodeFontProvider>());

        public static (string Code, FontType Font) ToAppFont(this string self)
        {
            return CodeFontProvider.GetCode(self);
        }

        public static bool ContainsAppFont(this string self)
        {
            return CodeFontProvider.Contains(self);
        }

        public static (bool hasColorDefinition, AppColorDefinition Color) AppColor(this string self)
        {
            return CodeFontProvider.GetColor(self);
        }
    }
}
