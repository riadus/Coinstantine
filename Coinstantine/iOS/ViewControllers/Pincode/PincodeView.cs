using System;
using Coinstantine.Core;
using Coinstantine.Core.ViewModels.Settings;
using Coinstantine.iOS.Converters;
using Coinstantine.iOS.Views.Extensions;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace Coinstantine.iOS
{
    public partial class PincodeView : MvxView
    {
        public PincodeView (IntPtr handle) : base (handle)
        {
        }

		private PincodeViewModel ViewModel => DataContext as PincodeViewModel;

		public override void AwakeFromNib()
		{
            base.AwakeFromNib();
            BackgroundColor = AppColorDefinition.MainBlue.ToUIColor();
			PinContainer.BackgroundColor = AppColorDefinition.MainBlue.ToUIColor();
            StyleButtons();
			ButtonsContainer.BackgroundColor = AppColorDefinition.White.ToUIColor();
			PinPanel1.BackgroundColor = UIColor.White;
			PinPanel2.BackgroundColor = UIColor.White;
			PinPanel3.BackgroundColor = UIColor.White;
			InformationLabel.TextColor = UIColor.White;
			LockLabel.TextColor = AppColorDefinition.MainBlue.ToUIColor();
            this.DelayBind(SetBindings);
		}

        private void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<PincodeView, PincodeViewModel>();

			bindingSet.Bind(LockLabel)
                      .For("AppString")
			          .To(vm => vm.LockLabel);

            bindingSet.Bind(Pin1View)
			          .For(v => v.BackgroundColor)
                      .To(vm => vm.Pin1Label)
			          .WithConversion(new PinConverter());
            bindingSet.Bind(Pin2View)
			          .For(v => v.BackgroundColor)
                      .To(vm => vm.Pin2Label)
			          .WithConversion(new PinConverter());
            bindingSet.Bind(Pin3View)
			          .For(v => v.BackgroundColor)
                      .To(vm => vm.Pin3Label)
			          .WithConversion(new PinConverter());
            bindingSet.Bind(Pin4View)
			          .For(v => v.BackgroundColor)
                      .To(vm => vm.Pin4Label)
			          .WithConversion(new PinConverter());
                      
            bindingSet.Bind(InformationLabel)
                      .To(vm => vm.InformationLabel);

            bindingSet.Bind(Pin0)
                      .To(vm => vm.PinCommand)
                      .CommandParameter(PincodeViewModel.Pin.Pin0);
            bindingSet.Bind(Pin1)
                      .To(vm => vm.PinCommand)
                      .CommandParameter(PincodeViewModel.Pin.Pin1);
            bindingSet.Bind(Pin2)
                      .To(vm => vm.PinCommand)
                      .CommandParameter(PincodeViewModel.Pin.Pin2);
            bindingSet.Bind(Pin3)
                      .To(vm => vm.PinCommand)
                      .CommandParameter(PincodeViewModel.Pin.Pin3);
            bindingSet.Bind(Pin4)
                      .To(vm => vm.PinCommand)
                      .CommandParameter(PincodeViewModel.Pin.Pin4);
            bindingSet.Bind(Pin5)
                      .To(vm => vm.PinCommand)
                      .CommandParameter(PincodeViewModel.Pin.Pin5);
            bindingSet.Bind(Pin6)
                      .To(vm => vm.PinCommand)
                      .CommandParameter(PincodeViewModel.Pin.Pin6);
            bindingSet.Bind(Pin7)
                      .To(vm => vm.PinCommand)
                      .CommandParameter(PincodeViewModel.Pin.Pin7);
            bindingSet.Bind(Pin8)
                      .To(vm => vm.PinCommand)
                      .CommandParameter(PincodeViewModel.Pin.Pin8);
            bindingSet.Bind(Pin9)
                      .To(vm => vm.PinCommand)
                      .CommandParameter(PincodeViewModel.Pin.Pin9);
            bindingSet.Bind(PinDel)
                      .To(vm => vm.PinCommand)
                      .CommandParameter(PincodeViewModel.Pin.Del);
            bindingSet.Bind(PinDel)
                      .For("AppString")
                      .To(vm => vm.DeleteText);
            
            bindingSet.Bind(PinFingerPrint)
                     .To(vm => vm.PinCommand)
                     .CommandParameter(PincodeViewModel.Pin.FingerPrint);
            bindingSet.Bind(PinFingerPrint)
                      .For(v => v.Hidden)
                      .To(vm => vm.ShowFingerPrintButton)
                      .WithRevertedConversion();
			bindingSet.Bind(PinFingerPrint)
					  .For("AppString")
					  .To(vm => vm.FingerPrintText);
            bindingSet.Bind(PinFingerPrint)
                      .For("TitleColor")
                      .To(vm => vm.FingerPrintEnabled)
                      .WithConversion(new BoolToColorWithDefaultConverter(AppColorDefinition.SecondaryColor.ToUIColor(), AppColorDefinition.Red.ToUIColor(), AppColorDefinition.MainBlue.ToUIColor(), () => ViewModel.FingerPrintToUnlock));

            bindingSet.Apply();

			ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private static void ToPinButton(UIButton button)
        {
            var mainColor = AppColorDefinition.MainBlue.ToUIColor();
            button.SetTitleColor(mainColor, UIControlState.Normal);
			button.BackgroundColor = UIColor.White;
			button.Superview.BackgroundColor = mainColor;
			button.Layer.CornerRadius = 10;
        }

        private void StyleButtons()
        {
            StyleButtons(ButtonsContainer);
        }

        private void StyleButtons(UIView subview)
        {
            foreach (var view in subview.Subviews)
            {
                if (view is UIButton)
                {
                    ToPinButton(view as UIButton);
                }
                else
                {
                    StyleButtons(view);
                }
            }
        }

		void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(ViewModel.Error))
			{
				ProgessionView.BackgroundColor = UIColor.Red;
				ProgessionView.ShakeHorizontally();
				ProgessionView.BackgroundColor = UIColor.White;
			}
		}
	}
}