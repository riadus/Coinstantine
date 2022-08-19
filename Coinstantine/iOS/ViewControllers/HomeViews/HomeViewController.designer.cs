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
    [Register ("HomeViewController")]
    partial class HomeViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Coinstantine.iOS.BaseScrollView ContentView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint TopConstraint { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ContentView != null) {
                ContentView.Dispose ();
                ContentView = null;
            }

            if (TopConstraint != null) {
                TopConstraint.Dispose ();
                TopConstraint = null;
            }
        }
    }
}