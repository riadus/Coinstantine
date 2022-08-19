using System;
using Coinstantine.Core;
using Coinstantine.Core.Converters;
using Coinstantine.Core.ViewModels.Home;
using Coinstantine.iOS.Converters;
using Coinstantine.iOS.Views;
using Coinstantine.iOS.Views.Extensions;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace Coinstantine.iOS
{
    public partial class BuyView : MvxView
    {
        private ConversionTextfieldsView ConversionTextfieldsView { get; set; }
        public BuyView (IntPtr handle) : base (handle)
        {
        }

        private BuyViewModel ViewModel => DataContext as BuyViewModel;

		public override void WillMoveToSuperview(UIView newsuper)
		{
			base.WillMoveToSuperview(newsuper);
            AddGestureRecognizer(new UITapGestureRecognizer(TapAction));

            Separator1.BackgroundColor = UIColor.Clear;
            Separator2.BackgroundColor = UIColor.Clear;
            Separator3.BackgroundColor = UIColor.Clear;
            Separator4.BackgroundColor = AppColorDefinition.MainBlue.ToUIColor();

            CsnPriceInETH.TextColor = AppColorDefinition.MainBlue.ToUIColor();
            CsnPriceInDollar.TextColor = AppColorDefinition.MainBlue.ToUIColor();

            BackToPrincipalButton.SetTitleColor(AppColorDefinition.MainBlue.ToUIColor(), UIControlState.Normal);

            BonusContainer.BackgroundColor = AppColorDefinition.SecondaryColor.ToUIColor().ColorWithAlpha(0.3f);
            TotalInETH.TextColor = AppColorDefinition.SecondaryColor.ToUIColor();
            TotalCsn.TextColor = AppColorDefinition.SecondaryColor.ToUIColor();

            BuyCSNButton.BackgroundColor = UIColor.Black;
            BuyCSNButton.TitleLabel.AdjustsFontSizeToFitWidth = true;
            BuyCSNButton.TitleLabel.MinimumScaleFactor = 0.3f;
            BuyCSNButton.SetTitleColor(AppColorDefinition.SecondaryColor.ToUIColor(), UIControlState.Normal);

            InfoCharcaterLabel.TextColor = AppColorDefinition.SecondaryColor.ToUIColor();

            InfoLabel.TextColor = UIColor.Black;

            BottomBackground.BackgroundColor = AppColorDefinition.MainBlue.ToUIColor();

		}
        private bool _built;
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            ButtonContainer.BackgroundColor = UIColor.Black;
            ButtonContainer.Layer.CornerRadius = ButtonContainer.Frame.Width / 2;
            BuyCSNButton.Layer.CornerRadius = BuyCSNButton.Frame.Width / 2;
            if (_built) { return; }
            ConversionTextfieldsView = ViewFactory.Create<ConversionTextfieldsView>();
            ConversionTextfieldsView.Frame = new CoreGraphics.CGRect(0, 0, Frame.Width, 120);
            ConversionTextfieldsContainer.Add(ConversionTextfieldsView);
            ConversionTextfieldsView.Initialize();
            SetBindings();
            _built = true;
        }

        private void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<BuyView, BuyViewModel>();
            bindingSet.Bind(BackToPrincipalButton)
                      .To(vm => vm.BackToPrincipalCommand);
            bindingSet.Bind(BackToPrincipalButton)
                      .For("AppString")
                      .To(vm => vm.BackToPrincipalText);

            bindingSet.Bind(CsnValueLabel)
                      .To(vm => vm.CsnValueText);
            bindingSet.Bind(CsnPriceInETH)
                      .To(vm => vm.CsnPriceInETHText);
            bindingSet.Bind(CsnPriceInDollar)
                      .To(vm => vm.CsnPriceDollarText);

            bindingSet.Bind(ConversionTextfieldsView.AmountTextfield)
                      .For(v => v.Placeholder)
                      .To(vm => vm.CoinstantineAmountText);
            bindingSet.Bind(ConversionTextfieldsView.AmountTextfield)
                      .To(vm => vm.CoinstantineAmount);

            bindingSet.Bind(ConversionTextfieldsView.CostTextfield)
                     .For(v => v.Placeholder)
                     .To(vm => vm.CoinstantineCostText);
            bindingSet.Bind(ConversionTextfieldsView.CostTextfield)
                      .To(vm => vm.CoinstantineCost);

            bindingSet.Bind(ConversionTextfieldsView)
                      .For(v => v.GeneralColor)
                      .To(vm => vm.CorrectInput)
                      .WithConversion(new BoolToColorConverter(AppColorDefinition.MainBlue, AppColorDefinition.Error, c => c.ToUIColor()));

            bindingSet.Bind(BonusLabel)
                      .To(vm => vm.BonusText);
            
            bindingSet.Bind(TotalLabel)
                      .To(vm => vm.TotalLabel);
            bindingSet.Bind(TotalInETH)
                      .To(vm => vm.TotalInETHText);
            bindingSet.Bind(TotalCsn)
                      .To(vm => vm.TotalCsnText);

            bindingSet.Bind(InfoCharcaterLabel)
                      .For("AppString")
                      .To(vm => vm.InfoCharacter);
            bindingSet.Bind(InfoCharcaterLabel)
                      .For(v => v.TextColor)
                      .To(vm => vm.Status)
                      .WithConversion(new StatusToColorConverter(c => c.ToUIColor()));

            bindingSet.Bind(InfoLabel)
                      .To(vm => vm.InfoLabel);

            bindingSet.Bind(BuyCSNButton)
                      .To(vm => vm.BuyCommand);
            bindingSet.Bind(BuyCSNButton)
                      .For("CSN")
                      .To(vm => vm.BuyCsnText);

            bindingSet.Apply();
        }

        void TapAction()
        {
            ConversionTextfieldsView.CostTextfield.ResignFirstResponder();
            ConversionTextfieldsView.AmountTextfield.ResignFirstResponder();
        }
    }
}