using System;
using CoreGraphics;
using Coinstantine.Core;
using Coinstantine.Core.Extensions;
using UIKit;

namespace Coinstantine.iOS.Views.Extensions
{
    public static class UIColorExtensions
    {
        public static UIColor ToUIColor(this AppColorDefinition self)
        {
            var appColor = self.ToAppColor();

            if (appColor.Alpha == null)
            {
                return UIColor.FromRGB(appColor.Red / 255f, appColor.Green / 255f, appColor.Blue / 255f);

            }
            return UIColor.FromRGBA((nfloat)appColor.Red / 255f, (nfloat)appColor.Green / 255f, (nfloat)appColor.Blue / 255f, (nfloat)appColor.Alpha / 255f);
        }

		public static CGColor ToCGColor(this AppColorDefinition self)
		{
			return self.ToUIColor().CGColor;
		}
    }
}
