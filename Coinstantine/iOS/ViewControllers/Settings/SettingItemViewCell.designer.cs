// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Coinstantine.iOS.Views.Settings
{
    [Register ("SettingItemViewCell")]
    partial class SettingItemViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView IconContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel IconLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Label { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView NotValidatedView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ValidatedLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (IconContainer != null) {
                IconContainer.Dispose ();
                IconContainer = null;
            }

            if (IconLabel != null) {
                IconLabel.Dispose ();
                IconLabel = null;
            }

            if (Label != null) {
                Label.Dispose ();
                Label = null;
            }

            if (NotValidatedView != null) {
                NotValidatedView.Dispose ();
                NotValidatedView = null;
            }

            if (ValidatedLabel != null) {
                ValidatedLabel.Dispose ();
                ValidatedLabel = null;
            }
        }
    }
}