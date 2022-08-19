using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using UIKit;

namespace Coinstantine.iOS.CustomBindings
{
    public class TextColorButtonBinding : MvxConvertingTargetBinding<UIButton, UIColor>
    {
        public TextColorButtonBinding(UIButton target) : base(target)
        {
        }

        protected override void SetValueImpl(UIButton target, UIColor value)
        {
            if (target == null || value == null) return;
            target.TintColor = value;
            target.SetTitleColor(value, UIControlState.Normal);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;
    }
}
