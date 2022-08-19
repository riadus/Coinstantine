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
    [Register ("LandingPageViewController")]
    partial class LandingPageViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel CoinstantineLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CreateAccountButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton LoginButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CoinstantineLabel != null) {
                CoinstantineLabel.Dispose ();
                CoinstantineLabel = null;
            }

            if (CreateAccountButton != null) {
                CreateAccountButton.Dispose ();
                CreateAccountButton = null;
            }

            if (LoginButton != null) {
                LoginButton.Dispose ();
                LoginButton = null;
            }
        }
    }
}