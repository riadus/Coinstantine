using System;
using System.Globalization;
using Coinstantine.Core;
using Coinstantine.iOS.Views.Extensions;
using MvvmCross.Converters;
using UIKit;

namespace Coinstantine.iOS.Converters
{
    public class AppColorConverter : MvxValueConverter<AppColorDefinition, UIColor>
    {
        protected override UIColor Convert(AppColorDefinition value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToUIColor();
        }
    }
}
