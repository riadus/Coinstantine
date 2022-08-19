using System;
using System.Globalization;
using MvvmCross.Converters;

namespace Coinstantine.iOS.Converters
{
    public class BoolToFontSize : IMvxValueConverter
    {
        private readonly nfloat _trueSize;
        private readonly nfloat _falseSize;

        public BoolToFontSize() { }

        public BoolToFontSize(nfloat trueSize, nfloat falseSize)
        {
            _trueSize = trueSize;
            _falseSize = falseSize;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? _trueSize : _falseSize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
