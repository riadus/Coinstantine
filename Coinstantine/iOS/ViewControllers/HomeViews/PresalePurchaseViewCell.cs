using System;
using Coinstantine.Core;
using Coinstantine.Core.ViewModels.Home;
using Coinstantine.iOS.Views.Extensions;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace Coinstantine.iOS.ViewControllers.HomeViews
{
    public partial class PresalePurchaseViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("PresalePurchaseViewCell");
        public static readonly UINib Nib;

        static PresalePurchaseViewCell()
        {
            Nib = UINib.FromName("PresalePurchaseViewCell", NSBundle.MainBundle);
        }

        protected PresalePurchaseViewCell(IntPtr handle) : base(handle)
        {
            this.DelayBind(SetBindings);
        }

        private void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<PresalePurchaseViewCell, PurchaseItemViewModel>();

            bindingSet.Bind(PurchasePhase)
                      .To(vm => vm.PurchasePhase);
            bindingSet.Bind(AmountLabel)
                      .To(vm => vm.AmountBoughtStr);
            bindingSet.Bind(CostLabel)
                      .To(vm => vm.CostStr);
            bindingSet.Bind(StatusLabel)
                      .For("AppString")
                      .To(vm => vm.StatusLabel);
            bindingSet.Bind(BubbleLabel)
                      .To(vm => vm.ButtonText);
            bindingSet.Bind(PurchaseDateLabel)
                      .To(vm => vm.PurchaseDateStr);
            bindingSet.Bind(BubbleButton)
                      .To(vm => vm.SelectedCommand);
            
            bindingSet.Apply();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            ContainerView.Layer.ShadowRadius = 2.0f;
            ContainerView.Layer.ShadowColor = UIColor.Black.CGColor;
            ContainerView.Layer.ShadowOffset = new CGSize(-1, 1);
            ContainerView.Layer.ShadowOpacity = 0.5f;
            ContainerView.Layer.MasksToBounds = false;
            ContainerView.BackgroundColor = UIColor.White;
            BubbleStatus.BackgroundColor = AppColorDefinition.MainBlue.ToUIColor();
            BubbleLabel.TextColor = AppColorDefinition.MainBlue.ToUIColor();
            BubbleLabel.AdjustsFontSizeToFitWidth = true;
            BubbleLabel.MinimumScaleFactor = 0.3f;
            StatusLabel.Font = UIFont.FromName("FontAwesome5Free-Solid", StatusLabel.Font.PointSize);
            CostLabel.AdjustsFontSizeToFitWidth = true;
            CostLabel.MinimumScaleFactor = 0.3f;
            SelectionStyle = UITableViewCellSelectionStyle.None;
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            BubbleStatus.Layer.CornerRadius = BubbleStatus.Frame.Width / 2;
        }
    }
}
