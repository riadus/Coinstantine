using Android.Util;
using Android.Widget;
using MvvmCross.Binding.Bindings.Target;

namespace Coinstantine.Droid.CustomBindings
{
    public class TextViewTextSizeBinding : MvxConvertingTargetBinding<TextView, int>
    {
        public TextViewTextSizeBinding(TextView target) : base(target)
        {
        }

        protected override void SetValueImpl(TextView target, int value)
        {
            if (target == null)
            {
                return;
            }
            target.SetTextSize(ComplexUnitType.Sp, value);
        }
    }
}
