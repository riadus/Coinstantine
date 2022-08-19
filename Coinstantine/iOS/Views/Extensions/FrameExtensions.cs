using CoreGraphics;
using UIKit;

namespace Coinstantine.iOS.Views.Extensions
{
    public static class FrameExtensions
    {
        public static void SetSize(this UIView view, CGSize size)
		{
			view.Frame = new CGRect(view.Frame.Location, size);
		}

        public static void SetPosition(this UIView view, CGPoint position)
		{
			view.Frame = new CGRect(position, view.Frame.Size);
		}

		public static void CenterHorizontally(this UIView view, UIView parentView)
		{
			var width = view.Frame.Width;
			var x = (parentView.Frame.Width - width) / 2;
			view.SetPosition(new CGPoint(x, view.Frame.Y));
		}

        public static void PutInCenterOf(this UIView view, UIView parentView)
        {
            view.Center = new CGPoint(parentView.Frame.Width / 2, parentView.Frame.Height / 2);
        }

        public static void PlaceAtBottomOf(this UIView view, UIView parentView)
		{
			view.SetPosition(new CGPoint(view.Frame.X, parentView.Frame.Height - view.Frame.Height));
		}

		public static void PlaceMoreDown(this UIView view, float extra)
        {
			view.SetPosition(new CGPoint(view.Frame.X, view.Frame.Y + extra));
        }

        public static void SetX(this UIView view, float x)
		{
			view.Frame = new CGRect(x, view.Frame.Y, view.Frame.Width, view.Frame.Height);
		}

		public static void SetY(this UIView view, float y)
        {
            view.Frame = new CGRect(view.Frame.X, y, view.Frame.Width, view.Frame.Height);
        }

        public static void WiderThan(this UIView view, CGSize size, float extraWidth)
		{
			view.SetSize(new CGSize(size.Width + extraWidth, view.Frame.Height));
		}

        public static void HigherThan(this UIView view, CGSize size, float extraHeight)
		{
			view.SetSize(new CGSize(view.Frame.Width, size.Height + extraHeight));
		}

        public static CGSize SizeThatFitsWithMargin(this UIView view, CGSize size, float margin)
        {
            var sizeWithoutMargin = view.SizeThatFits(size);
            if (sizeWithoutMargin.Width >= size.Width * 0.75)
            {
                return new CGSize(sizeWithoutMargin.Width - margin, sizeWithoutMargin.Height);
            }
            return sizeWithoutMargin;
        }

        public static void ToCircle(this UIView self)
        {
            self.Layer.CornerRadius = self.Frame.Width / 2;
        }
    }
}
