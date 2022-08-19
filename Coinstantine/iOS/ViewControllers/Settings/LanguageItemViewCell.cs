using System;
using Coinstantine.Core;
using Coinstantine.Core.ViewModels.Settings;
using Coinstantine.iOS.Converters;
using Coinstantine.iOS.Views.Extensions;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace Coinstantine.iOS.Views.Settings
{
    public partial class LanguageItemViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("LanguageItemViewCell");
        public static readonly UINib Nib;

        static LanguageItemViewCell()
        {
            Nib = UINib.FromName("LanguageItemViewCell", NSBundle.MainBundle);
        }

        protected LanguageItemViewCell(IntPtr handle) : base(handle)
        {
            this.DelayBind(SetBindings);
        }

        private void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<LanguageItemViewCell, LanguageItemViewModel>();

            bindingSet.Bind(LanguageText)
                      .To(vm => vm.LanguageText);
            bindingSet.Bind(LanguageText)
                      .For("FontSize")
                      .To(vm => vm.IsSelected)
                      .WithConversion(new BoolToFontSize(19, 15));
            
            bindingSet.Bind(SelectedOverlay)
                      .For(v => v.Hidden)
                      .To(vm => vm.IsSelected)
                      .WithRevertedConversion();

            bindingSet.Apply();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            SelectionStyle = UITableViewCellSelectionStyle.None;
            LanguageText.TextColor = AppColorDefinition.MainBlue.ToUIColor();
            SelectedOverlay.BackgroundColor = AppColorDefinition.MainBlue.ToUIColor();
            FlagContainer.BackgroundColor = AppColorDefinition.MainBlue.ToUIColor();
            SelectedOverlay.ToCircle();
            FlagContainer.ToCircle();
        }
    }
}
