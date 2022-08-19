using System;
using System.Globalization;
using Android.Graphics;
using Coinstantine.Core;
using Coinstantine.Common;
using MvvmCross.Converters;
using Coinstantine.Droid.Extensions;

namespace Coinstantine.Droid.Converters
{
    public class PinConverter : MvxValueConverter<string, Color>
    {
        protected override Color Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.IsNullOrEmpty() ? Color.White : AppColorDefinition.SecondaryColor.ToAndroidColor();
        }
    }
}
