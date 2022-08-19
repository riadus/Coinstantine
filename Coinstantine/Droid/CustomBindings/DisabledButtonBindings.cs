using Android.Graphics;
using Android.Widget;
using MvvmCross.Binding.Bindings.Target;

namespace Coinstantine.Droid.CustomBindings
{
    public class DisabledButtonBindings : MvxConvertingTargetBinding<Button, bool>
    {
        public DisabledButtonBindings(Button target) : base(target)
        {
        }

        protected override void SetValueImpl(Button target, bool value)
        {
            if (target == null || value)
            {
                return;
            }
            if (value)
            {
                target.SetTextColor(Color.Gray);
                target.Clickable = false;
                target.Enabled = false;
            }
            else
            {
                target.SetTextColor(Color.White);
                target.Clickable = true;
                target.Enabled = true;
            }
        }
    }
}