using Android.Content;
using Android.Views;
using Android.Widget;
using Coinstantine.Core;
using Coinstantine.Core.ViewModels.Settings;
using Coinstantine.Droid.CustomViews.BindableLayouts;
using Coinstantine.Droid.CustomViews.IconViews;
using Coinstantine.Droid.Extensions;
using MvvmCross.Binding.BindingContext;

namespace Coinstantine.Droid.CustomViews.Settings
{
    public class ProfileValidationItem : BindableLinearLayout
    {
        protected IconTextView Icon { get; set; }
        protected TextView Text { get; set; }
        protected View NotCheckedView { get; set; }
        protected TextView CheckedView { get; set; }

        public ProfileValidationItem(Context context) : base(context)
        {
            this.DelayBind(SetBindings);
            Initialize();
        }

        private void Initialize()
        {
            Inflate(Context, Resource.Layout.profile_validation_item, this);
            Icon = FindViewById<IconTextView>(Resource.Id.profile_validation_item_icon);
            Text = FindViewById<TextView>(Resource.Id.profile_validation_item_textview);
            NotCheckedView = FindViewById<View>(Resource.Id.profile_validation_item_not_checked);
            CheckedView = FindViewById<TextView>(Resource.Id.profile_validation_item_checked);

            NotCheckedView.ToCircle(Context, 10.ToDP(Context), AppColorDefinition.Error.ToAndroidColor());
            CheckedView.Text = "check";
            CheckedView.Typeface = "check".ToTypeface();
            CheckedView.SetTextColor(AppColorDefinition.SecondaryColor.ToAndroidColor());
        }

        private void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<ProfileValidationItem, SettingItemViewModel>();

            bindingSet.Bind(Icon)
                      .For("AppString")
                      .To(vm => vm.IconTitle);
            bindingSet.Bind(Text)
                      .To(vm => vm.SettingTitle);
            bindingSet.Bind(NotCheckedView)
                      .For(v => v.Visibility)
                      .To(vm => vm.NotValidated)
                      .WithVisibilityGoneConversion();
            bindingSet.Bind(CheckedView)
                      .For(v => v.Visibility)
                      .To(vm => vm.Validated)
                      .WithVisibilityGoneConversion();
            bindingSet.Apply();
        }
    }
}
