using Android.Graphics;
using Coinstantine.Core.Fonts;
using Plugin.CurrentActivity;
using Coinstantine.Core.Extensions;

namespace Coinstantine.Droid.Extensions
{
    public static class AppStringExtension
    {
        public static Typeface ToTypeface(this FontType self)
        {
            var fontFamily = string.Empty;
            switch (self)
            {
                case FontType.FontAwesomeBrandNegative:
                case FontType.FontAwesomeBrand:
                    fontFamily = "Font Awesome 5 Brands-Regular-400.otf";
                    break;
                case FontType.FontAwesomeSolidSupport:
                    fontFamily = "Font Awesome 5 Free-Solid-900 support.otf";
                    break;
                case FontType.FontAwesomeSolid:
                    fontFamily = "Font Awesome 5 Free-Solid-900.otf";
                    break;
                case FontType.FontAwesomeRegular:
                    fontFamily = "Font Awesome 5 Free-Regular-400.otf";
                    break;
                case FontType.IcoMoon:
                    fontFamily = "icomoon.ttf";
                    break;
                case FontType.SanFrancisco:
                default:
                    return Typeface.Default;
            }
            return Typeface.CreateFromAsset(CrossCurrentActivity.Current.Activity.Assets, fontFamily);
        }

        public static Typeface ToTypeface(this string self)
        {
            var (Code, Font) = self.ToAppFont();
            return Font.ToTypeface();
        }

        public static string ToCode(this string self)
        {
            if (self.ContainsAppFont())
            {
                return self.ToAppFont().Code;
            }
            return self;
        }
    }
}
