using Android.Graphics;
using Android.Text;
using Android.Text.Style;
using Android.Widget;
using Coinstantine.Core;
using Coinstantine.Core.ViewModels.Home;
using Coinstantine.Droid.Extensions;
using Java.Lang;
using MvvmCross.Binding.Bindings.Target;

namespace Coinstantine.Droid.CustomBindings
{
    public class BuyCoinstantineButtonBinding : MvxConvertingTargetBinding<Button, AttributedName>
    {
        public BuyCoinstantineButtonBinding(Button target) : base(target)
        {
        }

        protected override void SetValueImpl(Button target, AttributedName value)
        {
            if(target == null) 
            {
                return;
            }
            var str = $"{value.WholeText}";
            str = str.Replace(value.SpecificText, value.SpecificText.ToCode());
            var icomoonColorTextAttribute = new ForegroundColorSpan(AppColorDefinition.SecondaryColor.ToAndroidColor());
            var icomoonTypeFaceTextAttribute = new AppTypefaceSpan("", value.SpecificText.ToTypeface());
            var icomoonFontSizeTextAttribute = new RelativeSizeSpan(0.9f);

            var textAttribute = new ForegroundColorSpan(AppColorDefinition.SecondaryColor.ToAndroidColor());

            var normalTextAttribute = new ForegroundColorSpan(Color.White);

            var prettyString = new SpannableString(str);
            prettyString.SetSpan(normalTextAttribute, 0, value.Config.Start, SpanTypes.ExclusiveExclusive);
            prettyString.SetSpan(icomoonColorTextAttribute, value.Config.Start, value.Config.End, SpanTypes.ExclusiveExclusive);
            prettyString.SetSpan(icomoonTypeFaceTextAttribute, value.Config.Start, value.Config.End, SpanTypes.ExclusiveExclusive);
            prettyString.SetSpan(icomoonFontSizeTextAttribute, value.Config.Start, value.Config.End, SpanTypes.ExclusiveExclusive);
            prettyString.SetSpan(textAttribute, value.Config.End, value.Config.End + 2, SpanTypes.ExclusiveExclusive);
            target.TextAlignment = Android.Views.TextAlignment.Center;
            target.SetText(prettyString, TextView.BufferType.Spannable);
        }
    }

    public class AppTypefaceSpan : TypefaceSpan
    {
        private readonly Typeface _typeface;

        public AppTypefaceSpan(string family, Typeface typeface) : base(family)
        {
            _typeface = typeface;
        }

        public override void UpdateDrawState(TextPaint ds)
        {
            Apply(ds);
        }

        public override void UpdateMeasureState(TextPaint paint)
        {
            Apply(paint);
        }

        private void Apply(Paint paint)
        {
            var oldTypeface = paint.Typeface;
            var oldStyle = oldTypeface != null ? oldTypeface.Style : 0;
            var fakeStyle = oldStyle & _typeface.Style;

            if ((fakeStyle & TypefaceStyle.Bold) != 0)
            {
                paint.FakeBoldText = true;
            }

            if ((fakeStyle & TypefaceStyle.Italic) != 0)
            {
                paint.TextSkewX = -0.25f;
            }

            paint.SetTypeface(_typeface);
        }
    }
}
