using System;
using Coinstantine.Core;
using Coinstantine.Core.ViewModels.Settings;
using Coinstantine.iOS.Views.Extensions;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace Coinstantine.iOS.Views.Settings
{
    public partial class SettingItemViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("SettingItemViewCell");
        public static readonly UINib Nib;

        static SettingItemViewCell()
        {
            Nib = UINib.FromName("SettingItemViewCell", NSBundle.MainBundle);
        }

        protected SettingItemViewCell(IntPtr handle) : base(handle)
        {
            this.DelayBind(SetBindings);
        }

        private void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<SettingItemViewCell, SettingItemViewModel>();

            bindingSet.Bind(IconLabel)
                      .For("AppStringReversed")
                      .To(vm => vm.IconTitle);
            bindingSet.Bind(Label)
                      .To(vm => vm.SettingTitle);
            
            bindingSet.Bind(NotValidatedView)
                      .For(v => v.Hidden)
                      .To(vm => vm.NotValidated)
                      .WithRevertedConversion();
            bindingSet.Bind(ValidatedLabel)
                     .For(v => v.Hidden)
                     .To(vm => vm.Validated)
                     .WithRevertedConversion();

            bindingSet.Apply();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            BackgroundColor = UIColor.Clear;
            Label.TextColor = UIColor.White;
            ValidatedLabel.Text = "check";
            ValidatedLabel.Font = "check".ToUIFont(12);
            ValidatedLabel.TextColor = AppColorDefinition.SecondaryColor.ToUIColor();
            NotValidatedView.BackgroundColor = AppColorDefinition.Red.ToUIColor();

        }
    }
}
