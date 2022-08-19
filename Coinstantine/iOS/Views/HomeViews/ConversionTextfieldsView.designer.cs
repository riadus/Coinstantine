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
    [Register ("ConversionTextfieldsView")]
    partial class ConversionTextfieldsView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DownArrow { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField Textfield1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField Textfield2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel UpArrow { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (DownArrow != null) {
                DownArrow.Dispose ();
                DownArrow = null;
            }

            if (Textfield1 != null) {
                Textfield1.Dispose ();
                Textfield1 = null;
            }

            if (Textfield2 != null) {
                Textfield2.Dispose ();
                Textfield2 = null;
            }

            if (UpArrow != null) {
                UpArrow.Dispose ();
                UpArrow = null;
            }
        }
    }
}