using Coinstantine.Core.Theme;
using MvvmCross;
using Splat;

namespace Coinstantine.Core.Extensions
{
    public static class AppColorExtension
    {
        private static IThemeProvider _themeProvider;
        private static IThemeProvider ThemeProvider => _themeProvider ?? (_themeProvider = Mvx.Resolve<IThemeProvider>());

        public static SplatColor ToSplatColor(this AppColorDefinition self)
        {
            var color = ThemeProvider.GetAppColor(self);
            return color.Alpha.HasValue ? SplatColor.FromArgb(color.Alpha.Value, color.Red, color.Green, color.Blue) : SplatColor.FromArgb(color.Red, color.Green, color.Blue);
        }

        public static AppColor ToAppColor(this AppColorDefinition self)
        {
            return ThemeProvider.GetAppColor(self);
        }
    }
}
