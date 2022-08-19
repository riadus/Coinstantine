// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Coinstantine.iOS.ViewControllers.HomeViews
{
    [Register ("PresalePurchaseViewCell")]
    partial class PresalePurchaseViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel AmountLabel { get; set; }

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
        UIKit.UILabel CostLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel PurchaseDateLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel PurchasePhase { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel StatusLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AmountLabel != null) {
                AmountLabel.Dispose ();
                AmountLabel = null;
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

            if (CostLabel != null) {
                CostLabel.Dispose ();
                CostLabel = null;
            }

            if (PurchaseDateLabel != null) {
                PurchaseDateLabel.Dispose ();
                PurchaseDateLabel = null;
            }

            if (PurchasePhase != null) {
                PurchasePhase.Dispose ();
                PurchasePhase = null;
            }

            if (StatusLabel != null) {
                StatusLabel.Dispose ();
                StatusLabel = null;
            }
        }
    }
}