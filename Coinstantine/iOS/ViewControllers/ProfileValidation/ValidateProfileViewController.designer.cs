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
    [Register ("ValidateProfileViewController")]
    partial class ValidateProfileViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView ProfilesTableView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ProfilesTableView != null) {
                ProfilesTableView.Dispose ();
                ProfilesTableView = null;
            }
        }
    }
}