using Android.Graphics;
using Coinstantine.Core;
using Coinstantine.Core.Fonts;
using Coinstantine.Droid.CustomViews.IconViews;
using Coinstantine.Droid.Extensions;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target;

namespace Coinstantine.Droid.CustomBindings
{
    public class IconTextViewBinding : MvxConvertingTargetBinding<IconTextView, string>
    {
        public IconTextViewBinding(IconTextView target) : base(target)
        {
        }

        protected override void SetValueImpl(IconTextView target, string value)
        {
            if(target == null)
            {
                return;
            }
            var codeFontProvider = Mvx.Resolve<ICodeFontProvider>();
            var (Code, Font) = codeFontProvider.GetCode(value);
            target.Icon.Text = Code;
            target.Icon.Typeface = Font.ToTypeface();
            target.Icon.SetTextColor(AppColorDefinition.MainBlue.ToAndroidColor());
            target.Container.SetBackgroundColor(Color.White);
            target.CircleIcon();

            if (Font == FontType.FontAwesomeBrandNegative)
            {
                target.Icon.SetTextColor(Color.White);
                target.Container.SetBackgroundColor(AppColorDefinition.MainBlue.ToAndroidColor());
                target.BigIcon();
            }
        }
    }
}
