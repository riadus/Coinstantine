// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Plugin.Xablu.Walkthrough.Pages
{
    [Register ("VestaPage")]
    partial class VestaPage
    {
        [Outlet]
        UIKit.UIImageView BackgroundImageView { get; set; }


        [Outlet]
        UIKit.UIView BottomView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Description { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView Image { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Title { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BackgroundImageView != null) {
                BackgroundImageView.Dispose ();
                BackgroundImageView = null;
            }

            if (BottomView != null) {
                BottomView.Dispose ();
                BottomView = null;
            }

            if (Description != null) {
                Description.Dispose ();
                Description = null;
            }

            if (Image != null) {
                Image.Dispose ();
                Image = null;
            }

            if (Title != null) {
                Title.Dispose ();
                Title = null;
            }
        }
    }
}