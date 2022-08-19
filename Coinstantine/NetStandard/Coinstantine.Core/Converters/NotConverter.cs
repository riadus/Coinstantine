using System;
using System.Collections.Generic;
using System.Globalization;
using Coinstantine.Core.ViewModels.Home;
using MvvmCross.Converters;

namespace Coinstantine.Core.Converters
{
    public class NotConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !((bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToColorConverter : IMvxValueConverter
    {
        private readonly AppColorDefinition _trueColor;
        private readonly AppColorDefinition _falseColor;
        private readonly Func<AppColorDefinition, object> _conversionFunc;

        public BoolToColorConverter() { }

        public BoolToColorConverter(AppColorDefinition trueColor, AppColorDefinition falseColor, Func<AppColorDefinition, object> conversionFunc)
        {
            _trueColor = trueColor;
            _falseColor = falseColor;
            _conversionFunc = conversionFunc;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return _conversionFunc((bool)value ? _trueColor : _falseColor);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StatusToColorConverter : IMvxValueConverter
    {
        private readonly Dictionary<BuyViewModel.BuyStatus, AppColorDefinition> _colors;
        private readonly Func<AppColorDefinition, object> _conversionFunc;

        public StatusToColorConverter(Func<AppColorDefinition, object> conversionFunc)
        {
            _colors = new Dictionary<BuyViewModel.BuyStatus, AppColorDefinition>
            {
                {BuyViewModel.BuyStatus.Ok, AppColorDefinition.SecondaryColor},
                {BuyViewModel.BuyStatus.NotOk, AppColorDefinition.Error},
                {BuyViewModel.BuyStatus.NotEnoughEther, AppColorDefinition.MainBlue},
            };
            _conversionFunc = conversionFunc;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return _conversionFunc(_colors[(BuyViewModel.BuyStatus)value]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
