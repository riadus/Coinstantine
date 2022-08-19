using Coinstantine.Core.Fonts;
using Coinstantine.iOS.Views.Extensions;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target;

namespace Coinstantine.iOS.CustomBindings
{
    public class CircleAppStringLabelBinding : MvxConvertingTargetBinding<CircleAppStringLabel, string>
    {
        public CircleAppStringLabelBinding(CircleAppStringLabel target) : base(target)
        {
        }

        protected override void SetValueImpl(CircleAppStringLabel target, string value)
        {
            var codeFontProvider = Mvx.Resolve<ICodeFontProvider>();
            var (Code, Font) = codeFontProvider.GetCode(value);
            target.TitleLabel.Text = Code;
            target.TitleLabel.Font = Font.ToUIFont(target.TitleLabel.Font.PointSize);
            if (Font == FontType.FontAwesomeBrandNegative)
            {
                target.IsNegative = true;
            }
        }
    }
}
