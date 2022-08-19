
using Android.App;
using Android.OS;
using Coinstantine.Core.ViewModels.Settings;
using Coinstantine.Droid.CustomViews.Homepage;
using Coinstantine.Droid.CustomViews.Settings;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;

namespace Coinstantine.Droid.Activities.Settings
{
    [Activity(Label = "SettingsActivity", Theme = "@style/AppTheme")]
    public class SettingsActivity : BaseActivity<SettingsViewModel>
    {
        protected override bool WithSpecialBackground => true;
        private MvxListView SettingsList { get; set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SetContentView(Resource.Layout.settings);

            SettingsList = FindViewById<MvxListView>(Resource.Id.settingsListView);
            SettingsList.Adapter = new GenericAdapter<SettingItemViewModel, SettingsItem>(this, BindingContext, c => new SettingsItem(c));
            var bindingSet = this.CreateBindingSet<SettingsActivity, SettingsViewModel>();

            bindingSet.Bind(SettingsList)
                      .For(v => v.ItemsSource)
                      .To(vm => vm.Settings);
            bindingSet.Bind(SettingsList)
                      .For(v => v.ItemClick)
                      .To(vm => vm.SelectedSettingCommand);

            bindingSet.Apply();
        }
    }
}
