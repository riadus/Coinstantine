using Coinstantine.Core.Fonts;
using Coinstantine.iOS.Views.Extensions;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target;

namespace Coinstantine.iOS.CustomBindings
{
    public class AppFontCircleBinding : MvxConvertingTargetBinding<IconTextField, string>
    {
        public AppFontCircleBinding(IconTextField target) : base(target)
        {
        }

        protected override void SetValueImpl(IconTextField target, string value)
        {
            var codeFontProvider = Mvx.Resolve<ICodeFontProvider>();
            var (Code, Font) = codeFontProvider.GetCode(value);
            target.CircleAppStringLabel.TitleLabel.Text = Code;
            target.CircleAppStringLabel.TitleLabel.Font = Font.ToUIFont(target.CircleAppStringLabel.TitleLabel.Font.PointSize);
            if (Font == FontType.FontAwesomeBrandNegative)
            {
                target.CircleAppStringLabel.IsNegative = true;
            }
        }
    }
}
