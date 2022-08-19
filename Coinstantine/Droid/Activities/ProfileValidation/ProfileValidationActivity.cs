
using Android.App;
using Coinstantine.Core.ViewModels;
using Coinstantine.Core.ViewModels.Settings;
using Coinstantine.Droid.CustomViews.Homepage;
using Coinstantine.Droid.CustomViews.Settings;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;

namespace Coinstantine.Droid.Activities.ProfileValidation
{
    [Activity(Label = "ProfileValidationActivity", Theme = "@style/AppTheme")]
    public class ProfileValidationActivity : BaseActivity<ValidateProfileViewModel>
    {
        private MvxListView _profileItemList;

        protected override void OnResume()
        {
            base.OnResume();
            if(_profileItemList != null)
            {
            }
        }

        protected override bool WithSpecialBackground => true;
        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SetContentView(Resource.Layout.settings);
            _profileItemList = FindViewById<MvxListView>(Resource.Id.settingsListView);
            _profileItemList.ChoiceMode = Android.Widget.ChoiceMode.Single;

            var bindingSet = this.CreateBindingSet<ProfileValidationActivity, ValidateProfileViewModel>();
            var adapter = new GenericAdapter<SettingItemViewModel, ProfileValidationItem>(this, BindingContext, c => new ProfileValidationItem(c));
            _profileItemList.Adapter = adapter;
            bindingSet.Bind(_profileItemList)
                      .For(v => v.ItemsSource)
                      .To(vm => vm.UserProfileItems);
            bindingSet.Bind(_profileItemList)
                      .For(v => v.ItemClick)
                      .To(vm => vm.SelectedThirdPartyItemCommand);


            bindingSet.Apply();
        }
    }
}
