using Coinstantine.Core;
using Coinstantine.Core.ViewModels.ProfileValidation;
using Coinstantine.iOS.Views.Extensions;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace Coinstantine.iOS.ViewControllers.ProfileValidation
{
    public partial class TelegramViewController : BaseViewController<TelegramViewModel>
    {
        public TelegramViewController() : base("TelegramViewController", null)
        {
        }

		protected override bool WithSpecialBackground => true;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			UsernameTextfield.ShouldReturn = (sender) =>
                                    {
                                    	sender.ResignFirstResponder();
                                    	return false;
                                    };

            StartConversationButton.BackgroundColor = UIColor.Black;
            StartConversationButton.SetTitleColor(UIColor.White, UIControlState.Normal);
            StartConversationButton.TitleLabel.AdjustsFontSizeToFitWidth = true;
            StartConversationButton.TitleEdgeInsets = new UIEdgeInsets(0, 10, 0, 10);

            TutorialButton.BackgroundColor = MainBlueColor;
            TutorialButton.SetTitleColor(UIColor.White, UIControlState.Normal);
            TutorialButton.TitleLabel.AdjustsFontSizeToFitWidth = true;
            TutorialButton.TitleEdgeInsets = new UIEdgeInsets(0, 10, 0, 10);
		}

        protected override void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<TelegramViewController, TelegramViewModel>();

			bindingSet.Bind(UsernameTextfield)
					  .To(vm => vm.Username);
			bindingSet.Bind(UsernameTextfield)
					  .For(v => v.Placeholder)
					  .To(vm => vm.UsernamePlaceholder);

			bindingSet.Bind(ExplanationLabel)
					  .To(vm => vm.ExplanationText);

			bindingSet.Bind(TutorialButton)
					  .For("Title")
					  .To(vm => vm.TutorialButtonText);
            bindingSet.Bind(TutorialButton)
                      .To(vm => vm.TutorialButtonCommand);

			bindingSet.Bind(StartConversationButton)
					  .For("Title")
					  .To(vm => vm.StartConversationButtonText);
			bindingSet.Bind(StartConversationButton)
					  .To(vm => vm.StartConversationCommand);
			
            bindingSet.Apply();
        }
    }
}

