using Coinstantine.Core;
using Coinstantine.Core.Fonts;
using Coinstantine.iOS.Views.Extensions;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target;
using UIKit;

namespace Coinstantine.iOS.CustomBindings
{
    public class CircleAppStringLabelReversedBinding : MvxConvertingTargetBinding<UILabel, string>
    {
        public CircleAppStringLabelReversedBinding(UILabel target) : base(target)
        {
        }

        protected override void SetValueImpl(UILabel target, string value)
        {
            var codeFontProvider = Mvx.Resolve<ICodeFontProvider>();
            var (Code, Font) = codeFontProvider.GetCode(value);
            target.Text = Code;
            target.Font = Font.ToUIFont(target.Font.PointSize);
            target.TextColor = AppColorDefinition.MainBlue.ToUIColor();
            target.Superview.BackgroundColor = UIColor.White;
            if (Font == FontType.FontAwesomeBrandNegative)
            {
                target.TextColor = UIColor.White;
                target.Superview.BackgroundColor = AppColorDefinition.MainBlue.ToUIColor();
                target.Font = Font.ToUIFont(30);
            }
        }
    }
}
