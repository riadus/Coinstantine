// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Coinstantine.iOS.Views.ProfileValidation
{
    [Register ("ProfileItemInfoTitleViewCell")]
    partial class ProfileItemInfoTitleViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Title { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Value { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Title != null) {
                Title.Dispose ();
                Title = null;
            }

            if (Value != null) {
                Value.Dispose ();
                Value = null;
            }
        }
    }
}