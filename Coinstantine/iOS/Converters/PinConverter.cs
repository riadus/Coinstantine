using System;
using System.Globalization;
using Coinstantine.Common;
using Coinstantine.Core;
using Coinstantine.iOS.Views.Extensions;
using MvvmCross.Converters;
using UIKit;

namespace Coinstantine.iOS.Converters
{
	public class PinConverter : IMvxValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var pinLabel = value?.ToString();
			if(pinLabel.IsNullOrEmpty())
			{
				return UIColor.White;
			}
			return AppColorDefinition.SecondaryColor.ToUIColor();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
