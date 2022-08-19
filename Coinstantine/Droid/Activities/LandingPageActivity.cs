using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Widget;
using Coinstantine.Core.ViewModels;
using Microsoft.Identity.Client;
using MvvmCross.Binding.BindingContext;

namespace Coinstantine.Droid.Activities
{
    [Activity(Label = "LandingPageActivity", Theme = "@style/AppTheme")]
    public class LandingPageActivity : BaseActivity<LandingPageViewModel>
    {
        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.landing_page);

            var loginButton = FindViewById<Button>(Resource.Id.landing_page_login_button);
            var createAccountButton = FindViewById<Button>(Resource.Id.landing_page_create_account_button);
            var appTitle = FindViewById<TextView>(Resource.Id.landingpage_app_title);
            var font = Typeface.CreateFromAsset(Assets, "Quicksand-Regular.otf");
            appTitle.Typeface = font;

            var bindingSet = this.CreateBindingSet<LandingPageActivity, LandingPageViewModel>();
            bindingSet.Bind(loginButton)
                      .For(v => v.Text)
                      .To(vm => vm.LoginButtonText);
            bindingSet.Bind(loginButton)
                      .To(vm => vm.LogonButtonCommand);

            bindingSet.Bind(createAccountButton)
                      .For(v => v.Text)
                      .To(vm => vm.CreateAccountButtonText);
            bindingSet.Bind(createAccountButton)
                      .To(vm => vm.CreateAccountButtonCommand);

            bindingSet.Apply();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
}
