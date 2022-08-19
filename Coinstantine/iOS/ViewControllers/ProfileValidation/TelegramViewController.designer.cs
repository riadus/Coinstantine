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

namespace Coinstantine.iOS.ViewControllers.ProfileValidation
{
    [Register ("TelegramViewController")]
    partial class TelegramViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ExplanationLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton StartConversationButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton TutorialButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Coinstantine.iOS.AutosizeTextField UsernameTextfield { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ExplanationLabel != null) {
                ExplanationLabel.Dispose ();
                ExplanationLabel = null;
            }

            if (StartConversationButton != null) {
                StartConversationButton.Dispose ();
                StartConversationButton = null;
            }

            if (TutorialButton != null) {
                TutorialButton.Dispose ();
                TutorialButton = null;
            }

            if (UsernameTextfield != null) {
                UsernameTextfield.Dispose ();
                UsernameTextfield = null;
            }
        }
    }
}