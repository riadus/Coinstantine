using System;
using System.Globalization;
using MvvmCross.Converters;
using UIKit;

namespace Coinstantine.iOS.Converters
{
    public class BoolToColorWithDefaultConverter : IMvxValueConverter
    {
        private readonly UIColor _trueColor;
        private readonly UIColor _falseColor;
        private readonly UIColor _defaultColor;
        private readonly Func<bool> _applyDefaultFunc;

        public BoolToColorWithDefaultConverter() { }

        public BoolToColorWithDefaultConverter(UIColor trueColor, UIColor falseColor, UIColor defaultColor, Func<bool> applyDefaultFunc)
        {
            _trueColor = trueColor;
            _falseColor = falseColor;
            _defaultColor = defaultColor;
            _applyDefaultFunc = applyDefaultFunc;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (_applyDefaultFunc())
            {
                return _defaultColor;
            }
            return (bool)value ? _trueColor : _falseColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
