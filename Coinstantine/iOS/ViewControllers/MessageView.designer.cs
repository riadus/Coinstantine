// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Coinstantine.iOS
{
    [Register ("MessageView")]
    partial class MessageView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ActualView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CancelButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel MessageLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton OkAloneButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton OkButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ActualView != null) {
                ActualView.Dispose ();
                ActualView = null;
            }

            if (CancelButton != null) {
                CancelButton.Dispose ();
                CancelButton = null;
            }

            if (MessageLabel != null) {
                MessageLabel.Dispose ();
                MessageLabel = null;
            }

            if (OkAloneButton != null) {
                OkAloneButton.Dispose ();
                OkAloneButton = null;
            }

            if (OkButton != null) {
                OkButton.Dispose ();
                OkButton = null;
            }
        }
    }
}