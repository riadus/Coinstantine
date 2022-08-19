using Android.Graphics;
using Coinstantine.Core;
using Coinstantine.Core.Extensions;

namespace Coinstantine.Droid.Extensions
{
    public static class ColorExtensions
    {
        public static Color ToAndroidColor(this AppColorDefinition self)
        {
            var appColor = self.ToAppColor();

            if (appColor.Alpha == null)
            {
                return new Color(appColor.Red, appColor.Green, appColor.Blue);

            }
            return new Color(appColor.Red, appColor.Green, appColor.Blue, appColor.Alpha.Value);
        }
    }
}
