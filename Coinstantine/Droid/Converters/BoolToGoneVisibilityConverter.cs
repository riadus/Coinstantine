using System;
using System.Globalization;
using Android.Views;
using MvvmCross.Converters;

namespace Coinstantine.Droid.Converters
{
    public class BoolToGoneVisibilityConverter : MvxValueConverter<bool, ViewStates>
    {
        protected override ViewStates Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ? ViewStates.Visible : ViewStates.Gone;
        }
    }
}
