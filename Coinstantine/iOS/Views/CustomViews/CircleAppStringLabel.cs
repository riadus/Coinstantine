using Coinstantine.Core;
using Coinstantine.iOS.Views.Extensions;
using CoreGraphics;
using System;
using UIKit;

namespace Coinstantine.iOS
{
    public partial class CircleAppStringLabel : UIView
    {
        public CircleAppStringLabel(IntPtr handle) : base(handle)
        {
            TitleLabel = new UILabel();
        }

        public CircleAppStringLabel()
        {
            TitleLabel = new UILabel();
        }

        private UIColor MainColor = AppColorDefinition.MainBlue.ToUIColor();

        private bool _built;
        public bool IsNegative { get; set; }
        public bool IsReversed { get; set; }
        public UILabel TitleLabel { get; set; }
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            if (_built)
            {
                return;
            }
            /*if (IsReversed)
            {
                BackgroundColor = UIColor.White;
                TitleLabel.TextColor = MainColor;
            }
            else
            {
                BackgroundColor = MainColor;
                TitleLabel.TextColor = UIColor.White;
            }*/
            SetColors(AppColorDefinition.MainBlue.ToUIColor());
            this.SetSize(new CGSize(Frame.Width, Frame.Width));
            TitleLabel.Font = TitleLabel.Font.WithSize(12);
            var size = TitleLabel.SizeThatFits(Frame.Size);
            TitleLabel.SetSize(size);
            TitleLabel.PutInCenterOf(this);
            Layer.CornerRadius = Frame.Width / 2;
            if (IsNegative)
            {
                SetNegative();
            }
            Add(TitleLabel);
            _built = true;
        }

        private void SetNegative()
        {
            /*if (IsReversed)
            {
                BackgroundColor = color;
                TitleLabel.TextColor = UIColor.White;
            }
            else
            {
                BackgroundColor = UIColor.White;
                TitleLabel.TextColor = color;
            }*/

            TitleLabel.Font = TitleLabel.Font.WithSize(24);
            var size = TitleLabel.SizeThatFits(Frame.Size);
            TitleLabel.SetSize(size);
            TitleLabel.PutInCenterOf(this);
        }

        private void SetColors(UIColor color)
        {
            if (IsNegative)
            {
                if (IsReversed)
                {
                    BackgroundColor = color;
                    TitleLabel.TextColor = UIColor.White;
                }
                else
                {
                    BackgroundColor = UIColor.White;
                    TitleLabel.TextColor = color;
                }
            }
            else
            {
                if (IsReversed)
                {
                    BackgroundColor = UIColor.White;
                    TitleLabel.TextColor = color;
                }
                else
                {
                    BackgroundColor = color;
                    TitleLabel.TextColor = UIColor.White;
                }
            }
        }

        private void BuildReversed()
        {
           
            this.SetSize(new CGSize(Frame.Width, Frame.Width));
            Layer.CornerRadius = Frame.Width / 2;
            TitleLabel.Font = TitleLabel.Font.WithSize(12);
            var size = TitleLabel.SizeThatFits(Frame.Size);
            TitleLabel.SetSize(size);
            TitleLabel.PutInCenterOf(this);
            if (IsNegative)
            {
                SetNegative();
            }
            Add(TitleLabel);
        }

        public void ChangeStatus(bool isError)
        {
            var color = isError ? AppColorDefinition.Red : AppColorDefinition.MainBlue;
            SetColors(color.ToUIColor());
        }
    }
}
