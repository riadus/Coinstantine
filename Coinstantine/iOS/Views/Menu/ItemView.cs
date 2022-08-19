using System;
using Coinstantine.Core;
using Coinstantine.Core.ViewModels;
using Coinstantine.iOS.Views.Extensions;
using CoreGraphics;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace Coinstantine.iOS.Views.Menu
{
    public class ItemView : MvxView
    {
        public nfloat IconWidth { get; }

        public CGPoint PreferedLocation { get; set; }

        public double RightLimit { get; set; }

		private MenuItemViewModel ViewModel => DataContext as MenuItemViewModel;

        public ItemView(nfloat iconWidth)
        {
            IconWidth = iconWidth;
        }

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			if (ViewModel.SelectionCommand != null)
			{
				AddGestureRecognizer(new UITapGestureRecognizer(() => ViewModel.SelectionCommand.Execute()));
			}

			var icon = new ItemIcon
			{
				Frame = new CGRect(CGPoint.Empty, new CGSize(IconWidth, IconWidth)),
				Text = ViewModel.IconText.ToCode(),
				TextAlignment = UITextAlignment.Center,
				Enabled = ViewModel.IsEnabled,
				Font = ViewModel.IconText.ToUIFont(16),
				TextColor = AppColorDefinition.MainBlue.ToUIColor()
			};

            var label = new UILabel
            {
                Text = ViewModel.Text,
                Enabled = ViewModel.IsEnabled,
                TextColor = UIColor.White,
                Font = UIFont.BoldSystemFontOfSize(16),
                Lines = 0
			};
			var labelSize = label.SizeThatFits(CGSize.Empty);
			var labelLocation = new CGPoint(IconWidth + 10, 3);
            if(labelSize.Width + labelLocation.X > RightLimit / 2)
            {
                labelSize.Width = ((nfloat) RightLimit / 2f) - labelLocation.X;
                labelSize.Height *= 2;
            }

			label.Frame = new CGRect(labelLocation, labelSize);
			Frame = new CGRect(PreferedLocation, new CGSize(IconWidth + labelSize.Width + 50, labelSize.Height + 20));
			Add(icon);
			Add(label);
		}
	}
}
