using Coinstantine.Core.ViewModels.ProfileValidation;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace Coinstantine.iOS.ViewControllers
{
    public partial class TwitterViewController : BaseViewController<TwitterViewModel>
    {
        public TwitterViewController() : base("TwitterViewController", null)
        {
        }

		protected override bool WithSpecialBackground => true;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            TweetButton.BackgroundColor = UIColor.Black;
            TweetButton.SetTitleColor(UIColor.White, UIControlState.Normal);
            TweetButton.TitleLabel.AdjustsFontSizeToFitWidth = true;
            TweetButton.TitleEdgeInsets = new UIEdgeInsets(0, 10, 0, 10);
            UpdateContentView();
        }

        protected override void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<TwitterViewController, TwitterViewModel>();

            bindingSet.Bind(TwitterAccount)
                      .To(vm => vm.TwitterAccount);
            
            bindingSet.Bind(ExplanationLabel)
                      .To(vm => vm.ExplanationText);
            
            bindingSet.Bind(TweetButton)
                      .For("Title")
                      .To(vm => vm.TwitterButtonText);
            bindingSet.Bind(TweetButton)
                      .To(vm => vm.AuthenticateAndTweetCommand);
            
            bindingSet.Apply();
        }

        void UpdateContentView()
        {
            var contentRect = CGRect.Empty;
            foreach(var view in ContainerView.Subviews)
            {
                contentRect = contentRect.UnionWith(view.Frame);
            }
            ContainerView.ContentSize = new CGSize(ContainerView.ContentSize.Width, contentRect.Height + 30);
        }
	}
}
