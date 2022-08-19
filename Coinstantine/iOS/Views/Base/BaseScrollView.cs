using Foundation;
using System;
using UIKit;

namespace Coinstantine.iOS
{
    public partial class BaseScrollView : UIScrollView
    {
        public BaseScrollView (IntPtr handle) : base (handle)
        {
        }

        public UIViewController ParentViewController { get; set; }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            ParentViewController.TouchesMoved(touches, evt);
        }
    }
}