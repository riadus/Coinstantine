using System;
using UIKit;

namespace Coinstantine.iOS
{
    public partial class AutosizeTextField : UITextField
    {
        public AutosizeTextField()
        {
        }

        public AutosizeTextField(IntPtr handle) : base(handle)
        {
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            foreach (var view in Subviews)
            {
                if (view is UILabel label)
                {
                    label.MinimumScaleFactor = 0.3f;
                    label.AdjustsFontSizeToFitWidth = true;
                }
            }
        }
    }
}