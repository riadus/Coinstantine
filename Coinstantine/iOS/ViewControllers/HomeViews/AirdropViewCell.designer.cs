// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Coinstantine.iOS.Views.HomeViews
{
    [Register ("AirdropViewCell")]
    partial class AirdropViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel AdditionalInfoLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BubbleButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel BubbleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView BubbleStatus { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ContainerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LatestStatusLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel StatusLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TokenAmount { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TokenName { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AdditionalInfoLabel != null) {
                AdditionalInfoLabel.Dispose ();
                AdditionalInfoLabel = null;
            }

            if (BubbleButton != null) {
                BubbleButton.Dispose ();
                BubbleButton = null;
            }

            if (BubbleLabel != null) {
                BubbleLabel.Dispose ();
                BubbleLabel = null;
            }

            if (BubbleStatus != null) {
                BubbleStatus.Dispose ();
                BubbleStatus = null;
            }

            if (ContainerView != null) {
                ContainerView.Dispose ();
                ContainerView = null;
            }

            if (LatestStatusLabel != null) {
                LatestStatusLabel.Dispose ();
                LatestStatusLabel = null;
            }

            if (StatusLabel != null) {
                StatusLabel.Dispose ();
                StatusLabel = null;
            }

            if (TokenAmount != null) {
                TokenAmount.Dispose ();
                TokenAmount = null;
            }

            if (TokenName != null) {
                TokenName.Dispose ();
                TokenName = null;
            }
        }
    }
}