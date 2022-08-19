using System;
using System.Globalization;
using Android.Graphics;
using Coinstantine.Core;
using Coinstantine.Droid.Extensions;
using MvvmCross.Converters;

namespace Coinstantine.Droid.Converters
{
    public class AndroidColorConverter : MvxValueConverter<AppColorDefinition, Color>
    {
        protected override Color Convert(AppColorDefinition value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToAndroidColor();
        }
    }
}
