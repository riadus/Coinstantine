using System;
using Coinstantine.Core;
using Coinstantine.Core.ViewModels.Home;
using Coinstantine.iOS.ViewControllers.HomeViews;
using Coinstantine.iOS.Views.Extensions;
using Coinstantine.iOS.Views.HomeViews;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace Coinstantine.iOS
{
    public partial class PrincipalView : MvxView
    {
        public PrincipalView (IntPtr handle) : base (handle)
        {
        }

        public UIViewController ParentViewController { get; set; }

        private void SetBindings()
        {
            var balanceRefreshControl = new MvxUIRefreshControl();
            var listRefreshControl = new MvxUIRefreshControl();
            TopScrollView.AddSubview(balanceRefreshControl);
            AirdropsTableView.RefreshControl = listRefreshControl;

            var bindingSet = this.CreateBindingSet<PrincipalView, PrincipalViewModel>();
            bindingSet.Bind(UsernameLabel)
                      .To(vm => vm.UsernameLabel);
            bindingSet.Bind(EnvironmentLabel)
                      .To(vm => vm.Environment);
            bindingSet.Bind(EnvironmentLabel)
                      .For(v => v.Hidden)
                      .To(vm => vm.ShowEnvironment)
                      .WithRevertedConversion();
            bindingSet.Bind(EthAddressLabel)
                      .To(vm => vm.EthAddress);
            bindingSet.Bind(CoinstantineBalance)
                      .For(v => v.DataContext)
                      .To(vm => vm.CoinstantineBalance);
            bindingSet.Bind(EtherBalance)
                      .For(v => v.DataContext)
                      .To(vm => vm.EtherBalance);
            
            bindingSet.Bind(BuyCoinstantineButton)
                      .For("CSN")
                      .To(vm => vm.BuyCsnText);
            bindingSet.Bind(BuyCoinstantineButton)
                      .To(vm => vm.BuyCsnCommand);

            bindingSet.Bind(ShareAddressButton)
                      .For("AppString")
                      .To(vm => vm.ShareButtonText);
            bindingSet.Bind(ShareAddressButton)
                      .To(vm => vm.ShareCommand)
                      .CommandParameter(ShareAddressButton);

            bindingSet.Bind(AirdropLabel)
                      .To(vm => vm.AirdropsLabel);

            bindingSet.Bind(balanceRefreshControl)
                      .For(v => v.RefreshCommand)
                      .To(vm => vm.RefreshCommand);
            bindingSet.Bind(balanceRefreshControl)
                      .For(v => v.IsRefreshing)
                      .To(vm => vm.IsRefreshing);
            bindingSet.Bind(balanceRefreshControl)
                      .For(v => v.Message)
                      .To(vm => vm.RefreshingMessage);

            bindingSet.Bind(listRefreshControl)
                      .For(v => v.RefreshCommand)
                      .To(vm => vm.RefreshListCommand);
            bindingSet.Bind(listRefreshControl)
                      .For(v => v.IsRefreshing)
                      .To(vm => vm.IsListLoading);
            bindingSet.Bind(listRefreshControl)
                      .For(v => v.Message)
                      .To(vm => vm.RefreshingListMessage);

            var source = new MainTableViewSource(AirdropsTableView);

            bindingSet.Bind(source)
                      .To(vm => vm.Airdrops);

            bindingSet.Apply();
            AirdropsTableView.ParentViewController = ParentViewController;
            AirdropsTableView.Source = source;
            AirdropsTableView.ReloadData();
            AirdropsTableView.TableFooterView = new UIView();
        }

        public override void WillMoveToSuperview(UIView newsuper)
        {
            base.WillMoveToSuperview(newsuper);
			UsernameLabel.TextColor = AppColorDefinition.SecondaryColor.ToUIColor();
            EthAddressLabel.AdjustsFontSizeToFitWidth = true;
            EthAddressLabel.MinimumScaleFactor = 0.3f;
            BackgroundColor = UIColor.Clear;
            EnvironmentLabel.TextColor = AppColorDefinition.Error.ToUIColor();
            BuyCoinstantineButton.TitleLabel.Lines = 1;
            BuyCoinstantineButton.TitleLabel.AdjustsFontSizeToFitWidth = true;
            BuyCoinstantineButton.TitleLabel.MinimumScaleFactor = 0.3f;

            SeparatorView.BackgroundColor = AppColorDefinition.MainBlue.ToUIColor();
            AirdropsTableView.RowHeight = (nfloat) Math.Min(130.0f, Frame.Height * 0.23f);

        }

        private bool _bound;

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            if (!_bound)
            {
                SetBindings();
                _bound = true;
            }
        }
    }

    public class MainTableViewSource : MvxTableViewSource
    {
        private static readonly NSString AirdropCellIdentifier = AirdropViewCell.Key;
        private static readonly NSString PresalePurchaseCellIdentifier = PresalePurchaseViewCell.Key;

        public MainTableViewSource(UITableView tableView)
            : base(tableView)
        {
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            tableView.RegisterNibForCellReuse(UINib.FromName(AirdropCellIdentifier, NSBundle.MainBundle),
                                              AirdropCellIdentifier);
            tableView.RegisterNibForCellReuse(UINib.FromName(PresalePurchaseCellIdentifier, NSBundle.MainBundle), PresalePurchaseCellIdentifier);
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath,
                                                              object item)
        {
            NSString cellIdentifier;
            if (item is AirdropItemViewModel)
            {
                cellIdentifier = AirdropCellIdentifier;
            }
            else
            {
                cellIdentifier = PresalePurchaseCellIdentifier;
            }
            return TableView.DequeueReusableCell(cellIdentifier, indexPath);
        }
    }
}