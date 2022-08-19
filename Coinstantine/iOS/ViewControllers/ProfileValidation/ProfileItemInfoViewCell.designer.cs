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
    [Register ("ProfileItemInfoViewCell")]
    partial class ProfileItemInfoViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Title1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Title2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Value1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Value2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Value3 { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Title1 != null) {
                Title1.Dispose ();
                Title1 = null;
            }

            if (Title2 != null) {
                Title2.Dispose ();
                Title2 = null;
            }

            if (Value1 != null) {
                Value1.Dispose ();
                Value1 = null;
            }

            if (Value2 != null) {
                Value2.Dispose ();
                Value2 = null;
            }

            if (Value3 != null) {
                Value3.Dispose ();
                Value3 = null;
            }
        }
    }
}