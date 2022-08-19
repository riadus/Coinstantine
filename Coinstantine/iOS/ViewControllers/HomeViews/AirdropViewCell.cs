using System;
using Coinstantine.Core;
using Coinstantine.Core.ViewModels.Home;
using Coinstantine.iOS.Views.Extensions;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace Coinstantine.iOS.Views.HomeViews
{
    public partial class AirdropViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("AirdropViewCell");
        public static readonly UINib Nib;

        static AirdropViewCell()
        {
            Nib = UINib.FromName("AirdropViewCell", NSBundle.MainBundle);
        }

        protected AirdropViewCell(IntPtr handle) : base(handle)
        {
            this.DelayBind(SetBindings);
        }

        private void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<AirdropViewCell, AirdropItemViewModel>();

            bindingSet.Bind(TokenName)
                      .To(vm => vm.AirdropTitle);
            bindingSet.Bind(TokenAmount)
                      .To(vm => vm.AmountToAirdropStr);
            bindingSet.Bind(LatestStatusLabel)
                      .To(vm => vm.LatestUpdate);
            bindingSet.Bind(StatusLabel)
                      .For("AppString")
                      .To(vm => vm.StatusLabel);
            bindingSet.Bind(BubbleLabel)
                      .To(vm => vm.ButtonText);
            bindingSet.Bind(AdditionalInfoLabel)
                      .To(vm => vm.AdditionalInfo);
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
            BubbleLabel.TextColor = AppColorDefinition.MainBlue.ToUIColor();
            BubbleLabel.AdjustsFontSizeToFitWidth = true;
            BubbleLabel.MinimumScaleFactor = 0.3f;
            StatusLabel.Font = StatusLabel.Font.SetSize(30);
            BubbleStatus.BackgroundColor = UIColor.White;
            if(StatusLabel.TextColor == UIColor.White)
            {
                BubbleStatus.BackgroundColor = AppColorDefinition.MainBlue.ToUIColor();
                StatusLabel.Font = StatusLabel.Font.SetSize(13);
            }

            AdditionalInfoLabel.AdjustsFontSizeToFitWidth = true;
            AdditionalInfoLabel.MinimumScaleFactor = 0.3f;
            SelectionStyle = UITableViewCellSelectionStyle.None;
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            BubbleStatus.Layer.CornerRadius = BubbleStatus.Frame.Width / 2;
        }
    }
}
