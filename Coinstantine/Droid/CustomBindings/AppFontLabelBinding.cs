using System;
using Android.Widget;
using Coinstantine.Common;
using Coinstantine.Core.Fonts;
using Coinstantine.Droid.Extensions;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target;

namespace Coinstantine.Droid.CustomBindings
{
    public class AppFontLabelBinding : MvxConvertingTargetBinding<TextView, string>
    {
        public AppFontLabelBinding(TextView target) : base(target)
        {
        }

        protected override void SetValueImpl(TextView target, string value)
        {
            try
            {
                if (target == null || value.IsNullOrEmpty())
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
            catch(Exception e)
             {
                var x = e.Message;
             }
        }
    }
}
