using Android.App;
using Android.Graphics;
using Android.Widget;
using Coinstantine.Core.ViewModels.ProfileValidation;
using Coinstantine.Droid.CustomViews;
using Coinstantine.Droid.Extensions;
using MvvmCross.Binding.BindingContext;

namespace Coinstantine.Droid.Activities.ProfileValidation
{
    [Activity(Label = "TwitterAccountValidationActivity", Theme = "@style/AppTheme")]
    public class TwitterAccountValidationActivity : BaseActivity<TwitterViewModel>
    {
        protected override bool WithSpecialBackground => true;

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SetContentView(Resource.Layout.validate_twitter_account);

            var twitterIdTextView = FindViewById<TextView>(Resource.Id.validate_twitter_account_twitter_id);
            var descriptionTextView = FindViewById<JustifiedTextView>(Resource.Id.validate_twitter_account_description);
            var checkAccountButton = FindViewById<Button>(Resource.Id.validate_twitter_account_start_button);

            var bindingSet = this.CreateBindingSet<TwitterAccountValidationActivity, TwitterViewModel>();

            bindingSet.Bind(twitterIdTextView)
                      .To(vm => vm.TwitterAccount);

            bindingSet.Bind(descriptionTextView)
                      .For("JustifiedText")
                      .To(vm => vm.ExplanationText);

            bindingSet.Bind(checkAccountButton)
                      .For(v => v.Text)
                      .To(vm => vm.TwitterButtonText);
            bindingSet.Bind(checkAccountButton)
                      .To(vm => vm.AuthenticateAndTweetCommand);

            bindingSet.Apply();

            checkAccountButton.SetCornerRadius(this, Color.Black);
            checkAccountButton.SetTextColor(Color.White);
        }
    }
}
