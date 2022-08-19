using System;
using Coinstantine.Core;
using Coinstantine.Core.ViewModels.Generic;
using Coinstantine.iOS.Views.Extensions;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace Coinstantine.iOS.Views.ProfileValidation
{
    public partial class ProfileItemInfoTitleViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("ProfileItemInfoTitleViewCell");
        public static readonly UINib Nib;

        static ProfileItemInfoTitleViewCell()
        {
            Nib = UINib.FromName("ProfileItemInfoTitleViewCell", NSBundle.MainBundle);
        }

        protected ProfileItemInfoTitleViewCell(IntPtr handle) : base(handle)
        {
            this.DelayBind(SetBindings);
        }

        public ProfileItemInfoTitleViewCell() : base(Key)
        {
            this.DelayBind(SetBindings);
            base.AwakeFromNib();
        }

        private void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<ProfileItemInfoTitleViewCell, GenericInfoItemViewModel>();
            bindingSet.Bind(Title)
                      .To(vm => vm.Title1);
            bindingSet.Bind(Value)
                      .To(vm => vm.Value1);
            bindingSet.Apply();
            Title.TextColor = AppColorDefinition.MainBlue.ToUIColor();
        }
    }
}
