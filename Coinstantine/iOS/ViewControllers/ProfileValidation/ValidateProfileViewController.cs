using Coinstantine.Core.ViewModels;
using Coinstantine.iOS.Views.Settings;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace Coinstantine.iOS.ViewControllers
{
    public partial class ValidateProfileViewController : BaseViewController<ValidateProfileViewModel>
    {
        public ValidateProfileViewController() : base("ValidateProfileViewController", null)
        {
        }

		protected override bool WithSpecialBackground => true;

        protected override void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<ValidateProfileViewController, ValidateProfileViewModel>();

            var source = new MvxSimpleTableViewSource(ProfilesTableView, SettingItemViewCell.Key, SettingItemViewCell.Key);
            bindingSet.Bind(source)
                      .To(vm => vm.UserProfileItems);
            bindingSet.Bind(source)
                      .For(s => s.SelectedItem)
                      .To(vm => vm.SelectedThirdPartyItem);
            bindingSet.Apply();

            ProfilesTableView.Source = source;
            ProfilesTableView.ReloadData();
            ProfilesTableView.TableFooterView = new UIView();
            ProfilesTableView.BackgroundColor = UIColor.Clear;
        }
    }
}

