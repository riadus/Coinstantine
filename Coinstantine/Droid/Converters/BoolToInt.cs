using System;
using System.Globalization;
using MvvmCross.Converters;

namespace Coinstantine.Droid.Converters
{
    public class BoolToInt : MvxValueConverter<bool, int>
    {
        private readonly int _trueSize;
        private readonly int _falseSize;

        public BoolToInt() { }

        public BoolToInt(int trueSize, int falseSize)
        {
            _trueSize = trueSize;
            _falseSize = falseSize;
        }

        protected override int Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ? _trueSize : _falseSize;
        }
    }
}
