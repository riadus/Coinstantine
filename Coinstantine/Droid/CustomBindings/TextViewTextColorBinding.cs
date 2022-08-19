using Android.Graphics;
using Android.Widget;
using MvvmCross.Binding.Bindings.Target;

namespace Coinstantine.Droid.CustomBindings
{
    public class TextViewTextColorBinding : MvxConvertingTargetBinding<TextView, Color>
    {
        public TextViewTextColorBinding(TextView target) : base(target)
        {
        }

        protected override void SetValueImpl(TextView target, Color value)
        {
            if(target == null)
            {
                return;
            }
            target.SetTextColor(value);
        }
    }
}
