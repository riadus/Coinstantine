using Android.App;
using Android.Graphics;
using Android.Widget;
using Coinstantine.Core.ViewModels.ProfileValidation;
using Coinstantine.Droid.CustomViews;
using Coinstantine.Droid.Extensions;
using MvvmCross.Binding.BindingContext;

namespace Coinstantine.Droid.Activities.ProfileValidation
{
    [Activity(Label = "BitcoinTalkAccountValidationActivity", Theme = "@style/AppTheme")]
    public class BitcoinTalkAccountValidationActivity : BaseActivity<BitcoinTalkProfileViewModel>
    {
        protected override bool WithSpecialBackground => true;

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SetContentView(Resource.Layout.validate_bct_account);

            var bctIdEditText = FindViewById<EditText>(Resource.Id.validate_bct_account_bct_id);
            var descriptionTextView = FindViewById<JustifiedTextView>(Resource.Id.validate_bct_account_description);
            var checkAccountButton = FindViewById<Button>(Resource.Id.validate_bct_account_start_button);

            var bindingSet = this.CreateBindingSet<BitcoinTalkAccountValidationActivity, BitcoinTalkProfileViewModel>();

            bindingSet.Bind(bctIdEditText)
                      .For(v => v.Hint)
                      .To(vm => vm.UserIdPlaceholder);
            bindingSet.Bind(bctIdEditText)
                      .To(vm => vm.UserId);

            bindingSet.Bind(descriptionTextView)
                      .For("JustifiedText")
                      .To(vm => vm.ExplanationText);

            bindingSet.Bind(checkAccountButton)
                      .For(v => v.Text)
                      .To(vm => vm.CheckButtonText);
            bindingSet.Bind(checkAccountButton)
                      .To(vm => vm.CheckButtonCommand);

            bindingSet.Apply();

            checkAccountButton.SetCornerRadius(this, Color.Black);
            checkAccountButton.SetTextColor(Color.White);
        }
    }
}
