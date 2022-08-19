using Coinstantine.Core.ViewModels.ProfileValidation;
using Coinstantine.iOS.ViewControllers;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace Coinstantine.iOS.Views.ProfileValidation
{
    public partial class BitcoinTalkViewController : BaseViewController<BitcoinTalkProfileViewModel>
    {
        protected override bool WithSpecialBackground => true;

        public BitcoinTalkViewController() : base("BitcoinTalkViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.BackgroundColor = MainBlueColor;

            View.AddGestureRecognizer(new UITapGestureRecognizer(DismissKeyboard));
            CheckProfileButton.BackgroundColor = UIColor.Black;
            CheckProfileButton.SetTitleColor(UIColor.White, UIControlState.Normal);
            CheckProfileButton.TitleLabel.AdjustsFontSizeToFitWidth = true;
            CheckProfileButton.TitleEdgeInsets = new UIEdgeInsets(0, 10, 0, 10);
        }

        protected override void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<BitcoinTalkViewController, BitcoinTalkProfileViewModel>();

            bindingSet.Bind(UserIdTextField)
                      .To(vm => vm.UserId);
            bindingSet.Bind(UserIdTextField)
                      .For(v => v.Placeholder)
                      .To(vm => vm.UserIdPlaceholder);

            bindingSet.Bind(ExplanationLabel)
                      .To(vm => vm.ExplanationText);

            bindingSet.Bind(CheckProfileButton)
                      .For("Title")
                      .To(vm => vm.CheckButtonText);
            bindingSet.Bind(CheckProfileButton)
                      .To(vm => vm.CheckButtonCommand);

            bindingSet.Apply();
        }

        void DismissKeyboard()
        {
            View.EndEditing(true);
        }
    }
}

