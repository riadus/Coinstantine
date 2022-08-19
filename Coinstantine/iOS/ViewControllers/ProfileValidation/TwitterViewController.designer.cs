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

namespace Coinstantine.iOS.ViewControllers
{
    [Register ("TwitterViewController")]
    partial class TwitterViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView ContainerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ExplanationLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton TweetButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TwitterAccount { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ContainerView != null) {
                ContainerView.Dispose ();
                ContainerView = null;
            }

            if (ExplanationLabel != null) {
                ExplanationLabel.Dispose ();
                ExplanationLabel = null;
            }

            if (TweetButton != null) {
                TweetButton.Dispose ();
                TweetButton = null;
            }

            if (TwitterAccount != null) {
                TwitterAccount.Dispose ();
                TwitterAccount = null;
            }
        }
    }
}