using Android.Graphics;
using Android.Widget;
using MvvmCross.Binding.Bindings.Target;

namespace Coinstantine.Droid.CustomBindings
{
    public class ButtonTextColorBinding : MvxConvertingTargetBinding<Button, Color>
    {
        public ButtonTextColorBinding(Button target) : base(target)
        {
        }

        protected override void SetValueImpl(Button target, Color value)
        {
            if(target == null)
            {
                return;
            }
            target.SetTextColor(value);
        }
    }
}