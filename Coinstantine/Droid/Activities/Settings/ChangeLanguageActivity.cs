
using Android.App;
using Android.Widget;
using Coinstantine.Core.ViewModels.Settings;
using Coinstantine.Droid.CustomViews.Homepage;
using Coinstantine.Droid.CustomViews.Settings;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;

namespace Coinstantine.Droid.Activities.Settings
{
    [Activity(Label = "ChangeLanguageActivity", Theme = "@style/AppTheme")]
    public class ChangeLanguageActivity : BaseActivity<ChangeLanguageViewModel>
    {
        protected override bool WithSpecialBackground => true;
        private MvxListView LanguagesList { get; set; }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SetContentView(Resource.Layout.settings_change_language);
            LanguagesList = FindViewById<MvxListView>(Resource.Id.settings_language_list);
            LanguagesList.Adapter = new GenericAdapter<LanguageItemViewModel, LanguageItem>(this, BindingContext, c => new LanguageItem(c));
            var bindingSet = this.CreateBindingSet<ChangeLanguageActivity, ChangeLanguageViewModel>();

            bindingSet.Bind(LanguagesList)
                      .For(v => v.ItemsSource)
                      .To(vm => vm.Languages);
            bindingSet.Bind(LanguagesList)
                      .For(v => v.SelectedItem)
                      .To(vm => vm.SelectedLanguage);

            bindingSet.Apply();
        }
    }
}
