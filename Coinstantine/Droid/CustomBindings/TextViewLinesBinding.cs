using Android.Widget;
using MvvmCross.Binding.Bindings.Target;

namespace Coinstantine.Droid.CustomBindings
{
    public class TextViewLinesBinding : MvxConvertingTargetBinding<TextView, int>
    {
        public TextViewLinesBinding(TextView target) : base(target)
        {
        }

        protected override void SetValueImpl(TextView target, int value)
        {
            if(target == null)
            {
                return;
            }
            target.SetLines(value);
        }
    }
}
