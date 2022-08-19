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
    [Register ("ProfileInfosView")]
    partial class ProfileInfosView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton EditButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView ItemInfosTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel RemainingTime { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel UserIdLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ValidateButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (EditButton != null) {
                EditButton.Dispose ();
                EditButton = null;
            }

            if (ItemInfosTableView != null) {
                ItemInfosTableView.Dispose ();
                ItemInfosTableView = null;
            }

            if (RemainingTime != null) {
                RemainingTime.Dispose ();
                RemainingTime = null;
            }

            if (UserIdLabel != null) {
                UserIdLabel.Dispose ();
                UserIdLabel = null;
            }

            if (ValidateButton != null) {
                ValidateButton.Dispose ();
                ValidateButton = null;
            }
        }
    }
}