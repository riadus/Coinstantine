using Android.Graphics;
using Android.Widget;
using MvvmCross.Binding.Bindings.Target;

namespace Coinstantine.Droid.CustomBindings
{
    public class DisabledTextViewBindings : MvxConvertingTargetBinding<TextView, bool>
    {
        public DisabledTextViewBindings(TextView target) : base(target)
        {
        }

        protected override void SetValueImpl(TextView target, bool value)
        {
            if (target == null || value)
            {
                return;
            }
            target.SetTextColor(Color.Gray);
        }
    }
}