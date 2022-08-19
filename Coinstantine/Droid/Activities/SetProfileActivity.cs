using Android.App;
using Android.Widget;
using Coinstantine.Core.ViewModels.Account.AccountCreation;
using Coinstantine.Core.ViewModels.Account.IconTextField;
using Coinstantine.Core.ViewModels.Account.Login;
using Coinstantine.Droid.CustomViews.IconViews;
using Coinstantine.Droid.Extensions;
using MvvmCross.Binding.BindingContext;

namespace Coinstantine.Droid.Activities
{
    [Activity(Label = "CreateAccountActivity", Theme = "@style/AppTheme")]
    public class CreateAccountActivity : IconTextfieldFormActivity<CreateAccountViewModel>
    { }

    [Activity(Label = "LoginActivity", Theme = "@style/AppTheme")]
    public class LoginActivity : IconTextfieldFormActivity<LoginViewModel>
    { }

    public class IconTextfieldFormActivity<T> : BaseActivity<T> where T : class, IIconTextfieldFormViewModel
    {
        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.icon_edittext_form);
            var iconEditTextContainer = FindViewById<IconEditTextContainer>(Resource.Id.icon_edittext_form_container);
            var button = FindViewById<Button>(Resource.Id.icon_edit_text_form_button);
            var progressBar = FindViewById<ProgressBar>(Resource.Id.icon_edit_text_progressbar);
            var bindingSet = this.CreateBindingSet<IconTextfieldFormActivity<T>, IIconTextfieldFormViewModel>();

            progressBar.Indeterminate = true;

            bindingSet.Bind(iconEditTextContainer)
                      .For(v => v.DataContext)
                      .To(vm => vm);
            bindingSet.Bind(button)
                      .For(v => v.Text)
                      .To(vm => vm.ButtonText);
            bindingSet.Bind(button)
                      .For("Enabled")
                      .To(vm => vm.IsLoading);
            bindingSet.Bind(button)
                      .To(vm => vm.ButtonCommand);
            bindingSet.Bind(progressBar)
                      .For(v => v.IsShown)
                      .To(vm => vm.IsLoading);
            bindingSet.Bind(progressBar)
                      .For(v => v.Visibility)
                      .To(vm => vm.IsLoading)
                      .WithVisibilityGoneConversion();
            bindingSet.Apply();
        }
    }
}
