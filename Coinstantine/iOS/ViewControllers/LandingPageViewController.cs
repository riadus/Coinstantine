using Coinstantine.Core.ViewModels;
using Coinstantine.iOS.Views.Extensions;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using UIKit;

namespace Coinstantine.iOS.ViewControllers
{
    [MvxRootPresentation(WrapInNavigationController = true)]
    public partial class LandingPageViewController : BaseViewController<LandingPageViewModel>
    {
        public LandingPageViewController() : base("LandingPageViewController", null)
        {
        }

		protected override bool WithSpecialBackground => false;
		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
            NavigationController.NavigationBar.Hidden = true;
            LoginButton.ToRevertedRegularStyle();
            CreateAccountButton.ToRegularStyle();
            CreateAccountButton.BackgroundColor = UIColor.Black;
            CoinstantineLabel.AdjustsFontSizeToFitWidth = true;
            CoinstantineLabel.MinimumScaleFactor = 0.3f;
        }

        protected override void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<LandingPageViewController, LandingPageViewModel>();

            bindingSet.Bind(LoginButton)
                      .For("Title")
                      .To(vm => vm.LoginButtonText);
            bindingSet.Bind(LoginButton)
                      .To(vm => vm.LogonButtonCommand);

            bindingSet.Bind(CreateAccountButton)
                      .For("Title")
                      .To(vm => vm.CreateAccountButtonText);
            bindingSet.Bind(CreateAccountButton)
                      .To(vm => vm.CreateAccountButtonCommand);
            bindingSet.Apply();
        }
    }
}

