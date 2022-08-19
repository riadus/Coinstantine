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
    [Register ("PrincipalView")]
    partial class PrincipalView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel AirdropLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Coinstantine.iOS.BaseTableView AirdropsTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BuyCoinstantineButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Coinstantine.iOS.TokenBalanceView CoinstantineBalance { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel EnvironmentLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel EthAddressLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Coinstantine.iOS.TokenBalanceView EtherBalance { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView SeparatorView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ShareAddressButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView TopScrollView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView TopView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel UsernameLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AirdropLabel != null) {
                AirdropLabel.Dispose ();
                AirdropLabel = null;
            }

            if (AirdropsTableView != null) {
                AirdropsTableView.Dispose ();
                AirdropsTableView = null;
            }

            if (BuyCoinstantineButton != null) {
                BuyCoinstantineButton.Dispose ();
                BuyCoinstantineButton = null;
            }

            if (CoinstantineBalance != null) {
                CoinstantineBalance.Dispose ();
                CoinstantineBalance = null;
            }

            if (EnvironmentLabel != null) {
                EnvironmentLabel.Dispose ();
                EnvironmentLabel = null;
            }

            if (EthAddressLabel != null) {
                EthAddressLabel.Dispose ();
                EthAddressLabel = null;
            }

            if (EtherBalance != null) {
                EtherBalance.Dispose ();
                EtherBalance = null;
            }

            if (SeparatorView != null) {
                SeparatorView.Dispose ();
                SeparatorView = null;
            }

            if (ShareAddressButton != null) {
                ShareAddressButton.Dispose ();
                ShareAddressButton = null;
            }

            if (TopScrollView != null) {
                TopScrollView.Dispose ();
                TopScrollView = null;
            }

            if (TopView != null) {
                TopView.Dispose ();
                TopView = null;
            }

            if (UsernameLabel != null) {
                UsernameLabel.Dispose ();
                UsernameLabel = null;
            }
        }
    }
}