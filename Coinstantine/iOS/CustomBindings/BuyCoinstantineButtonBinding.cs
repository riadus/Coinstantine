using Coinstantine.Core;
using Coinstantine.Core.ViewModels.Home;
using Coinstantine.iOS.Views.Extensions;
using Foundation;
using MvvmCross.Binding.Bindings.Target;
using UIKit;

namespace Coinstantine.iOS.CustomBindings
{
    public class BuyCoinstantineButtonBinding : MvxConvertingTargetBinding<UIButton, AttributedName>
    {
        public BuyCoinstantineButtonBinding(UIButton target) : base(target)
        {
        }

        protected override void SetValueImpl(UIButton target, AttributedName value)
        {
            var str = $"{value.WholeText}";
            str = str.Replace(value.SpecificText, value.SpecificText.ToCode());
            var icomoonTextAttribute = new UIStringAttributes
            {
                Font = value.SpecificText.ToUIFont(17),
                ForegroundColor = AppColorDefinition.SecondaryColor.ToUIColor()
            };

            var textAttribute = new UIStringAttributes
            {
                Font = UIFont.BoldSystemFontOfSize(20),
                ForegroundColor = AppColorDefinition.SecondaryColor.ToUIColor()
            };

            var normalTextAttribute = new UIStringAttributes
            {
                Font = UIFont.BoldSystemFontOfSize(20),
                ForegroundColor = UIColor.White
            };

            var prettyString = new NSMutableAttributedString(str);
            // Coloring the placeholder
            prettyString.SetAttributes(normalTextAttribute.Dictionary, new NSRange(0, value.Config.Start));
            prettyString.SetAttributes(icomoonTextAttribute.Dictionary, new NSRange(value.Config.Start, value.Config.End - value.Config.Start));
            prettyString.SetAttributes(textAttribute.Dictionary, new NSRange(value.Config.End, 2));
            target.TitleLabel.TextAlignment = UITextAlignment.Center;
            target.TitleLabel.Lines = 0;
            target.TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
            target.SetAttributedTitle(prettyString, UIControlState.Normal);
        }
    }
}
