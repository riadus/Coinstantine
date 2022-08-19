using Android.Widget;
using Coinstantine.Common;
using Coinstantine.Core.Fonts;
using Coinstantine.Droid.Extensions;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target;

namespace Coinstantine.Droid.CustomBindings
{
    public class AppFontButtonBinding : MvxConvertingTargetBinding<Button, string>
    {
        public AppFontButtonBinding(Button target) : base(target)
        {
        }

        protected override void SetValueImpl(Button target, string value)
        {
            if(target == null || value.IsNullOrEmpty())
            {
                return;
            }
            var codeFontProvider = Mvx.Resolve<ICodeFontProvider>();
            var (Code, Font) = codeFontProvider.GetCode(value);
            var (ColorApplies, Color) = codeFontProvider.GetColor(value);
            target.Text = Code;
            target.Typeface = Font.ToTypeface();
            if (ColorApplies)
            {
                target.SetTextColor(Color.ToAndroidColor());
            }
        }
    }
}
