using Android.Graphics;
using Coinstantine.Core;
using Coinstantine.Core.Fonts;
using Coinstantine.Droid.CustomViews.IconViews;
using Coinstantine.Droid.Extensions;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target;

namespace Coinstantine.Droid.CustomBindings
{
    public class IconEditTextBinding : MvxConvertingTargetBinding<IconEditText, string>
    {
        public IconEditTextBinding(IconEditText target) : base(target)
        {
        }

        protected override void SetValueImpl(IconEditText target, string value)
        {
            if (target == null)
            {
                return;
            }
            var codeFontProvider = Mvx.Resolve<ICodeFontProvider>();
            var (Code, Font) = codeFontProvider.GetCode(value);
            target.Icon.Text = Code;
            target.Icon.Typeface = Font.ToTypeface();
            target.IsBigIcon = Font == FontType.FontAwesomeBrandNegative;
            target.MainColor = AppColorDefinition.MainBlue.ToAndroidColor();
            target.SecondaryColor = Color.White;

            target.SetIcon();
        }
    }
}
