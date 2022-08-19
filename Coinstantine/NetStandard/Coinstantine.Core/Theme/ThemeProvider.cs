using System;
using System.Collections.Generic;
using Coinstantine.Common;
using Coinstantine.Common.Attributes;

namespace Coinstantine.Core.Theme
{
    [RegisterInterfaceAsLazySingleton]
    public class ThemeProvider : IThemeProvider
    {
        private readonly Dictionary<AppColorDefinition, string> _definitions;
        public ThemeProvider()
        {
            _definitions = new Dictionary<AppColorDefinition, string>
            {
				{AppColorDefinition.MainBlue, "#3498DB" },//05246B
                {AppColorDefinition.SecondaryColor, "#00FFBE"}, //87FEB8
                {AppColorDefinition.Red, "#F15A24"},
                {AppColorDefinition.Error, "#EF4B48"},
                {AppColorDefinition.Black, "#000000" },
                {AppColorDefinition.White, "#FFFFFF" },
                {AppColorDefinition.TutorialTitle, "#3498DB" },
                {AppColorDefinition.TutorialDescription, "#000000" },
                {AppColorDefinition.TutorialBackground, "#F0F0F0" },
                {AppColorDefinition.TutorialToolbarBackground, "#00ED1A3B" },
                {AppColorDefinition.TutorialToolbarText, "#00FFBE" },
                {AppColorDefinition.TutorialToolbarSelectedPage, "#00FFBE" },
                {AppColorDefinition.TutorialToolbarUnselectedPage, "#87FE88" },
                {AppColorDefinition.LightGray, "#555555"}
            };
        }

        public AppColor GetAppColor(AppColorDefinition colorDefinition)
        {
            var colorRgb = _definitions[colorDefinition];

            colorRgb = colorRgb.Replace("#", String.Empty);
            string r, g, b, a = null;
            if(colorRgb.Length == 6)
            {
                r = colorRgb.Substring(0, 2);
                g = colorRgb.Substring(2, 2);
                b = colorRgb.Substring(4, 2);
            }
            else if(colorRgb.Length == 8)
            {
                a = colorRgb.Substring(0, 2);
                r = colorRgb.Substring(2, 2);
                g = colorRgb.Substring(4, 2);
                b = colorRgb.Substring(6, 2);
            }
            else
            {
                throw new NotSupportedException();
            }
            return new AppColor
            {
                Alpha = HexToInt(a),
                Red = HexToInt(r).Value,
                Green = HexToInt(g).Value,
                Blue = HexToInt(b).Value
            };
        }

        private int? HexToInt(string hex)
        {
            if(hex.IsNullOrEmpty())
            {
                return null;
            }
            return int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
        }
    }
}
