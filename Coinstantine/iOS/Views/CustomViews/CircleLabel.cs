using System;
using System.ComponentModel;
using Foundation;
using MvvmCross.Commands;
using UIKit;

namespace Coinstantine.iOS
{
    [DesignTimeVisible(true)]
    public partial class CircleLabel : UILabel, IComponent
    {
        public CircleLabel (IntPtr handle) : base (handle)
        {
            EdgeInsets = new UIEdgeInsets(10, 10, 10, 10);
        }

		public CircleLabel()
        {
            EdgeInsets = new UIEdgeInsets(10, 10, 10, 10);
        }

        [Export("EdgeInsets"), Browsable(true)]
        public UIEdgeInsets EdgeInsets { get; set; }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            Layer.CornerRadius = Frame.Width / 2;
            Layer.BorderWidth = 0;
            BackgroundColor = UIColor.Clear;
            TextAlignment = UITextAlignment.Center;
        }

        public override void DrawText(CoreGraphics.CGRect rect)
        {
            base.DrawText(EdgeInsets.InsetRect(rect));
        }

        public override CoreGraphics.CGSize IntrinsicContentSize
        {
            get
            {
                var size = base.IntrinsicContentSize;
                size.Width += EdgeInsets.Left + EdgeInsets.Right;
                size.Height += EdgeInsets.Top + EdgeInsets.Bottom;
                var max = (nfloat) Math.Max(size.Width, size.Height);
                size.Width = max;
                size.Height = max;
                return size;
            }
        }
        IMvxCommand _tapCommand;

        public event EventHandler Disposed;

        public IMvxCommand TapCommand
        {
            get
            {
                return _tapCommand;
            }

            set
            {
                _tapCommand = value;
                SetAction();
            }
        }

        public ISite Site { get; set; }

        private void SetAction()
        {
            if (GestureRecognizers != null)
            {
                foreach (var gesture in GestureRecognizers)
                {
                    RemoveGestureRecognizer(gesture);
                }
            }
            var newGesture = new UITapGestureRecognizer(() => TapCommand.Execute());
            AddGestureRecognizer(newGesture);
            UserInteractionEnabled = true;
        }
    }
}