
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using Coinstantine.Core;
using Coinstantine.Core.ViewModels.ProfileValidation;
using Coinstantine.Droid.CustomViews;
using Coinstantine.Droid.Extensions;
using MvvmCross.Binding.BindingContext;

namespace Coinstantine.Droid.Activities.ProfileValidation
{
    [Activity(Label = "TelegramAccountValidationActivity", Theme = "@style/AppTheme")]
    public class TelegramAccountValidationActivity : BaseActivity<TelegramViewModel>
    {
        protected override bool WithSpecialBackground => true;

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SetContentView(Resource.Layout.validate_telegram_account);

            var telegramIdEditText = FindViewById<EditText>(Resource.Id.validate_telegram_account_telegram_id);
            var descriptionTextView = FindViewById<JustifiedTextView>(Resource.Id.validate_telegram_account_description);
            var startChatButton = FindViewById<Button>(Resource.Id.validate_telegram_account_start_button);
            var tutorialButton = FindViewById<Button>(Resource.Id.validate_telegram_account_tutorial_button);

            var bindingSet = this.CreateBindingSet<TelegramAccountValidationActivity, TelegramViewModel>();

            bindingSet.Bind(telegramIdEditText)
                      .For(v => v.Hint)
                      .To(vm => vm.UsernamePlaceholder);
            bindingSet.Bind(telegramIdEditText)
                      .To(vm => vm.Username);

            bindingSet.Bind(descriptionTextView)
                      .For("JustifiedText")
                      .To(vm => vm.ExplanationText);

            bindingSet.Bind(startChatButton)
                      .For(v => v.Text)
                      .To(vm => vm.StartConversationButtonText);
            bindingSet.Bind(startChatButton)
                      .To(vm => vm.StartConversationCommand);

            bindingSet.Bind(tutorialButton)
                      .For(v => v.Text)
                      .To(vm => vm.TutorialButtonText);
            bindingSet.Bind(tutorialButton)
                      .To(vm => vm.TutorialButtonCommand);

            bindingSet.Apply();

            tutorialButton.SetCornerRadius(this, AppColorDefinition.MainBlue.ToAndroidColor());
            tutorialButton.SetTextColor(Color.White);
            startChatButton.SetCornerRadius(this, Color.Black);
            startChatButton.SetTextColor(Color.White);
        }
    }
}
