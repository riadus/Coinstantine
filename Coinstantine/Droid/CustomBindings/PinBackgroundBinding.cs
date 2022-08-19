using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using MvvmCross.Binding.Bindings.Target;

namespace Coinstantine.Droid.CustomBindings
{
    public class PinBackgroundBinding : MvxConvertingTargetBinding<View, Color>
    {
        public PinBackgroundBinding(View target) : base(target)
        {
        }

        protected override void SetValueImpl(View target, Color value)
        {
            if(target == null)
            {
                return;
            }
            var background = target.Background as GradientDrawable;
            background.SetColor(value);
            background.SetStroke(3, value);
        }
    }
}
