using System;
using Coinstantine.iOS.Views.Extensions;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using UIKit;

namespace Coinstantine.iOS.CustomBindings
{
    public class FontSizeLabelBinding : MvxConvertingTargetBinding<UILabel, nfloat>
    {
        public FontSizeLabelBinding(UILabel target) : base(target)
        {
        }

        protected override void SetValueImpl(UILabel target, nfloat value)
        {
            if (target == null || value == 0) return;
            target.Font = target.Font.SetSize(value);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;
    }
}
