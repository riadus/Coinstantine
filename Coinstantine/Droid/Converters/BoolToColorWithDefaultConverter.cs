using System;
using System.Globalization;
using Android.Graphics;
using MvvmCross.Converters;

namespace Coinstantine.Droid.Converters
{
    public class BoolToColorWithDefaultConverter : MvxValueConverter<bool, Color>
    {
        private readonly Color _trueColor;
        private readonly Color _falseColor;
        private readonly Color _defaultColor;
        private readonly Func<bool> _applyDefaultFunc;

        public BoolToColorWithDefaultConverter() { }

        public BoolToColorWithDefaultConverter(Color trueColor, Color falseColor, Color defaultColor, Func<bool> applyDefaultFunc)
        {
            _trueColor = trueColor;
            _falseColor = falseColor;
            _defaultColor = defaultColor;
            _applyDefaultFunc = applyDefaultFunc;
        }

        protected override Color Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            if (_applyDefaultFunc())
            {
                return _defaultColor;
            }
            return value ? _trueColor : _falseColor;
        }
    }
}
