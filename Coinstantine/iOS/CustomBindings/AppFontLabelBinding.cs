using Coinstantine.Core.Fonts;
using Coinstantine.iOS.Views.Extensions;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target;
using UIKit;

namespace Coinstantine.iOS.CustomBindings
{
    public class AppFontLabelBinding : MvxConvertingTargetBinding<UILabel, string>
    {
        public AppFontLabelBinding(UILabel target) : base(target)
        {
        }

        protected override void SetValueImpl(UILabel target, string value)
        {
            var codeFontProvider = Mvx.Resolve<ICodeFontProvider>();
            var (Code, Font) = codeFontProvider.GetCode(value);
            var (ColorApplies, Color) = codeFontProvider.GetColor(value);
            target.Text = Code;
            target.Font = Font.ToUIFont(target.Font.PointSize);
            if(ColorApplies)
            {
                target.TextColor = Color.ToUIColor();
            }
        }
    }
}
