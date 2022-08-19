using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Coinstantine.Core;
using Coinstantine.Core.ViewModels.Settings;
using Coinstantine.Droid.Converters;
using Coinstantine.Droid.CustomViews.BindableLayouts;
using Coinstantine.Droid.Extensions;
using MvvmCross.Binding.BindingContext;

namespace Coinstantine.Droid.CustomViews.Settings
{
    public class SettingsItem : BindableConstraintLayout
    {
        protected View IconContainer { get; set; }
        protected TextView Icon { get; set; }
        protected TextView SettingTextView { get; set; }

        public SettingsItem(Context context) : base(context)
        {
            this.DelayBind(SetBindings);
            Initialize();
        }

        public SettingsItem(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        private void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<SettingsItem, SettingItemViewModel>();

            bindingSet.Bind(Icon)
                      .For("AppString")
                      .To(vm => vm.IconTitle);
            bindingSet.Bind(SettingTextView)
                      .To(vm => vm.SettingTitle);

            bindingSet.Apply();
        }

        private void Initialize()
        {
            Inflate(Context, Resource.Layout.settings_item, this);
            IconContainer = FindViewById<View>(Resource.Id.settingsItemIconContainer);
            Icon = FindViewById<TextView>(Resource.Id.settingsItemIcon);
            SettingTextView = FindViewById<TextView>(Resource.Id.settingsItemSettingTextView);
            IconContainer.ToCircle(Context, 30.ToDP(Context), Color.White);
        }
    }

    public class LanguageItem : BindableConstraintLayout
    {
        private View SelectedView { get; set; }
        private View NestedView { get; set; }
        private TextView LanguageText { get; set; }

        public LanguageItem(Context context) : base(context)
        {
            this.DelayBind(SetBindings);
            Initialize();
        }

        public LanguageItem(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        private void Initialize()
        {
            Inflate(Context, Resource.Layout.settings_language_item, this);

            SelectedView = FindViewById<View>(Resource.Id.setting_language_item_selected_view);
            NestedView = FindViewById<View>(Resource.Id.setting_language_item_selected_nested_view);
            LanguageText = FindViewById<TextView>(Resource.Id.setting_language_item_text);

            SelectedView.ToCircle(Context, 40, AppColorDefinition.MainBlue.ToAndroidColor());
            NestedView.ToCircle(Context, 33, AppColorDefinition.MainBlue.ToAndroidColor());
        }

        private void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<LanguageItem, LanguageItemViewModel>();

            bindingSet.Bind(SelectedView)
                      .For(v => v.Visibility)
                      .To(vm => vm.IsSelected)
                      .WithVisibilityConversion();

            bindingSet.Bind(LanguageText)
                      .To(vm => vm.LanguageText);

            bindingSet.Bind(LanguageText)
                      .For("TextSize")
                      .To(vm => vm.IsSelected)
                      .WithConversion(new BoolToInt(19, 15));

            bindingSet.Apply();
        }
    }
}
