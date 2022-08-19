using Foundation;
using Coinstantine.Core;
using UIKit;

namespace Coinstantine.iOS.Views.Extensions
{
    public static class ButtonExtensions
    {
		public static void ToRegularStyle(this UIButton button)
		{
			button.SetTitleColor(AppColorDefinition.White.ToUIColor(), UIControlState.Normal);
			button.BackgroundColor = AppColorDefinition.MainBlue.ToUIColor();
            button.TitleLabel.Font = UIFont.FromName("Quicksand-Bold", 27);
            button.Layer.CornerRadius = 20;
		}

		public static void ToRevertedRegularStyle(this UIButton button)
        {
			button.SetTitleColor(AppColorDefinition.MainBlue.ToUIColor(), UIControlState.Normal);
			button.BackgroundColor = AppColorDefinition.White.ToUIColor();
            button.TitleLabel.Font = UIFont.FromName("Quicksand-Bold", 27);
            button.Layer.CornerRadius = 20;
        }

		public static void FromUIButtonItem(this UIButton button, UIBarButtonSystemItem barButtonSystemItem)
		{
			var item = new UIBarButtonItem(barButtonSystemItem);
			var bar = new UIToolbar();
			bar.SetItems(new[] { item }, false);
			bar.SnapshotView(true);
			var itemViewKey = item.ValueForKey(new NSString("view"));
			var itemView = itemViewKey as UIView;
			UIImage image = null;
            foreach(var view in itemView)
			{
				if(view is UIButton barButton)
				{
					image = barButton.ImageView.Image;
				}
			}
			button.SetImage(image, UIControlState.Normal);
		}
    }
}
