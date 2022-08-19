using System;
using Coinstantine.Core.Extensions;
using UIKit;
using Coinstantine.Core.Fonts;
using System.Diagnostics;
using System.Linq;

namespace Coinstantine.iOS.Views.Extensions
{
    public static class AppStringExtension
    {
        public static UIFont ToUIFont(this FontType self, nfloat size)
        {
            var fontFamily = string.Empty;
            switch (self)
            {
                case FontType.FontAwesomeBrandNegative:
                case FontType.FontAwesomeBrand:
                    fontFamily = "Font Awesome 5 Brands";
                    break;
                case FontType.FontAwesomeSolidSupport:
                    fontFamily = "FontAwesome5FreeSolid";
                    break;
                case FontType.FontAwesomeSolid:
                    fontFamily = "FontAwesome5Free-Solid";
                    break;
                case FontType.FontAwesomeRegular:
                    fontFamily = "FontAwesome5FreeRegular";
                    break;
                case FontType.IcoMoon:
                    fontFamily = "icomoon";
                    break;
                case FontType.SanFrancisco:
                default:
                    return UIFont.SystemFontOfSize(size);
            }
            return UIFont.FromName(fontFamily, size);
        }

        public static UIFont ToUIFont(this string self, nfloat size)
        {
            var (Code, Font) = self.ToAppFont();
            return Font.ToUIFont(size);
        }

        public static string ToCode(this string self)
        {
            if (self.ContainsAppFont())
            {
                return self.ToAppFont().Code;
            }
            return self;
        }

        public static void ListAllFonts(this UIViewController self)
        {
            foreach (var fontFamily in UIFont.FamilyNames.OrderBy(x => x))
            {
                Debug.WriteLine($"* Family : {fontFamily}");
                foreach(var font in UIFont.FontNamesForFamilyName(fontFamily))
                {
                    Debug.WriteLine($"* --- Font : {font}");
                }
            }
        }

        public static UIFont SetSize(this UIFont self, nfloat size)
        {
            return UIFont.FromName(self.Name, size);
        }
    }
}
