using Coinstantine.Core.Fonts;
using Coinstantine.iOS.Views.Extensions;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target;
using UIKit;

namespace Coinstantine.iOS.CustomBindings
{
    public class AppFontButtonBinding : MvxConvertingTargetBinding<UIButton, string>
    {
        public AppFontButtonBinding(UIButton target) : base(target)
        {
        }

        protected override void SetValueImpl(UIButton target, string value)
        {
            var codeFontProvider = Mvx.Resolve<ICodeFontProvider>();
            var (Code, Font) = codeFontProvider.GetCode(value);
            target.SetTitle(Code, UIControlState.Normal);
            target.TitleLabel.Font = Font.ToUIFont(target.TitleLabel.Font.PointSize);
        }
    }
}
