using Coinstantine.Core.ViewModels.Settings;
using Coinstantine.iOS.ViewControllers;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace Coinstantine.iOS.Views.Settings
{
    public partial class SettingsViewController : BaseViewController<SettingsViewModel>
    {
        public SettingsViewController() : base("SettingsViewController", null)
        {
        }

        protected override bool WithSpecialBackground => true;

        protected override void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<SettingsViewController, SettingsViewModel>();

            var source = new MvxSimpleTableViewSource(SettingsTableView, SettingItemViewCell.Key, SettingItemViewCell.Key);
            bindingSet.Bind(source)
                      .To(vm => vm.Settings);
            bindingSet.Bind(source)
                      .For(s => s.SelectedItem)
                      .To(vm => vm.SelectedSetting);
            bindingSet.Apply();

            SettingsTableView.Source = source;
            SettingsTableView.ReloadData();
            SettingsTableView.TableFooterView = new UIView();
        }
    }
}

