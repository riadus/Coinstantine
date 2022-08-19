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
    [Register ("LanguageItemViewCell")]
    partial class LanguageItemViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView FlagContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LanguageText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView SelectedOverlay { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (FlagContainer != null) {
                FlagContainer.Dispose ();
                FlagContainer = null;
            }

            if (LanguageText != null) {
                LanguageText.Dispose ();
                LanguageText = null;
            }

            if (SelectedOverlay != null) {
                SelectedOverlay.Dispose ();
                SelectedOverlay = null;
            }
        }
    }
}