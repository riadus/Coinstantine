using System;
using Coinstantine.Core;
using Coinstantine.Core.ViewModels.Generic;
using Coinstantine.Core.ViewModels.ProfileValidation;
using Coinstantine.iOS.Views.Extensions;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace Coinstantine.iOS.Views.ProfileValidation
{
    public partial class ProfileItemInfoViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("ProfileItemInfoViewCell");
        public static readonly UINib Nib;

        static ProfileItemInfoViewCell()
        {
            Nib = UINib.FromName("ProfileItemInfoViewCell", NSBundle.MainBundle);
        }

        protected ProfileItemInfoViewCell(IntPtr handle) : base(handle)
        {
            this.DelayBind(SetBindings);
        }

        public ProfileItemInfoViewCell() : base(Key)
        {
            this.DelayBind(SetBindings);
            base.AwakeFromNib();
        }

        private void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<ProfileItemInfoViewCell, GenericInfoItemViewModel>();
            bindingSet.Bind(Title1)
                      .To(vm => vm.Title1);
            bindingSet.Bind(Value1)
                      .To(vm => vm.Value1);
            bindingSet.Bind(Title1)
                      .For(v => v.Lines)
                      .To(vm => vm.Value1Lines);
            bindingSet.Bind(Value1)
                      .For(v => v.Lines)
                      .To(vm => vm.Value1Lines);

            bindingSet.Bind(Title2)
                      .To(vm => vm.Title2);
            bindingSet.Bind(Title2)
                      .For(v => v.Hidden)
                      .To(vm => vm.ShowSecondTitle)
                      .WithRevertedConversion();
            bindingSet.Bind(Value2)
                      .To(vm => vm.Value2);
            bindingSet.Bind(Value2)
                      .For(v => v.Hidden)
                      .To(vm => vm.ShowSecondValue)
                      .WithRevertedConversion();

            bindingSet.Bind(Value3)
                      .For("AppString")
                      .To(vm => vm.Value3);
            bindingSet.Bind(Value3)
                      .For(v => v.Hidden)
                      .To(vm => vm.ShowThirdValue)
                      .WithRevertedConversion();

            bindingSet.Apply();
            RemoveLabelsIfNeeded();
        }

        private void ToTitle(UILabel label)
        {
            if (label == null)
            {
                return;
            }
            label.Font = UIFont.SystemFontOfSize(15);
            label.TextColor = AppColorDefinition.MainBlue.ToUIColor();
        }

        private void ToUserInfo(UILabel label)
        {
            if(label == null)
            {
                return;
            }
            label.Font = UIFont.SystemFontOfSize(12);
            label.TextColor = UIColor.Black;
            label.AdjustsFontSizeToFitWidth = true;
            label.MinimumScaleFactor = 0.3f;
        }

        public override void WillMoveToSuperview(UIView newsuper)
        {
            base.WillMoveToSuperview(newsuper);
            ToTitle(Title1);
            ToTitle(Title2);

            ToUserInfo(Value1);
            ToUserInfo(Value2);
        }

        private void RemoveLabelsIfNeeded()
        {
            var viewModel = DataContext as GenericInfoItemViewModel;
            if(!viewModel?.ShowSecondTitle ?? false)
            {
                Title2.RemoveFromSuperview();
                Title2 = null;
            }
            if(!viewModel?.ShowSecondValue ?? false)
            {
                Value2.RemoveFromSuperview();
                Value2 = null;
            }
            if (!viewModel?.ShowThirdValue ?? false)
            {
                Value3.RemoveFromSuperview();
                Value3 = null;
            }
        }
    }
}
