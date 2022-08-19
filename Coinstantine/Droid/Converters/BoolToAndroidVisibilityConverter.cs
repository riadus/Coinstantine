using System;
using System.Globalization;
using Android.Views;
using MvvmCross.Converters;

namespace Coinstantine.Droid.Converters
{
    public class BoolToAndroidVisibilityConverter : MvxValueConverter<bool, ViewStates>
    {
        private readonly bool _reverted;

        public BoolToAndroidVisibilityConverter(bool reverted)
        {
            _reverted = reverted;
        }

        protected override ViewStates Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            if(_reverted)
            {
                return value ? ViewStates.Invisible : ViewStates.Visible;
            }
            return value ? ViewStates.Visible : ViewStates.Invisible;
        }
    }
}
