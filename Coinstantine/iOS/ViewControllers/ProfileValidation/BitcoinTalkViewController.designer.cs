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

namespace Coinstantine.iOS.Views.ProfileValidation
{
    [Register ("BitcoinTalkViewController")]
    partial class BitcoinTalkViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CheckProfileButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ExplanationLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Coinstantine.iOS.AutosizeTextField UserIdTextField { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CheckProfileButton != null) {
                CheckProfileButton.Dispose ();
                CheckProfileButton = null;
            }

            if (ExplanationLabel != null) {
                ExplanationLabel.Dispose ();
                ExplanationLabel = null;
            }

            if (UserIdTextField != null) {
                UserIdTextField.Dispose ();
                UserIdTextField = null;
            }
        }
    }
}