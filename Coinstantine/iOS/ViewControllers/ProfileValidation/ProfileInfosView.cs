using Coinstantine.Core;
using Coinstantine.Core.Converters;
using Coinstantine.Core.ViewModels.Generic;
using Coinstantine.Core.ViewModels.ProfileValidation;
using Coinstantine.iOS.ViewControllers;
using Coinstantine.iOS.Views;
using Coinstantine.iOS.Views.Extensions;
using Coinstantine.iOS.Views.ProfileValidation;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross.Platforms.Ios.Views;
using System;
using UIKit;

namespace Coinstantine.iOS
{
    public partial class ProfileInfosView : BaseView
    {
        public ProfileInfosView(IntPtr handle) : base(handle)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            this.DelayBind(SetBindings);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            ItemInfosTableView.RowHeight = Frame.Height * 0.15f;

            UserIdLabel.TextColor = MainBlueColor;
            ToTitle(RemainingTime);
            if (EditButton != null)
            {
                EditButton.Layer.BorderColor = MainBlueColor.CGColor;
                EditButton.SetTitleColor(MainBlueColor, UIControlState.Normal);
                EditButton.Layer.BorderWidth = 2;
            }
        }

        private void ToTitle(UILabel label)
        {
            label.Font = UIFont.SystemFontOfSize(15);
            label.TextColor = MainBlueColor;
        }

        private IGenericInfoViewModel ViewModel => DataContext as IGenericInfoViewModel;

        private void SetBindings()
        {
            var refreshingControl = new MvxUIRefreshControl();
            var bindingSet = this.CreateBindingSet<ProfileInfosView, IGenericInfoViewModel>();
            bindingSet.Bind(UserIdLabel)
                      .To(vm => vm.InfoTitle);

            var source = new ProfileItemsTableViewSource(ItemInfosTableView);

            bindingSet.Bind(refreshingControl)
                      .For(v => v.RefreshCommand)
                      .To(vm => vm.RefreshCommand);
            bindingSet.Bind(refreshingControl)
                      .For(v => v.IsRefreshing)
                      .To(vm => vm.IsRefreshing);
            bindingSet.Bind(refreshingControl)
                      .For(v => v.Message)
                      .To(vm => vm.RefreshText);

            bindingSet.Bind(source)
                      .To(vm => vm.GenericInfoItems);

            bindingSet.Bind(ValidateButton)
                      .For("Title")
                      .To(vm => vm.PrincipalButtonText);    
            bindingSet.Bind(ValidateButton)
                      .To(vm => vm.PrincipalButtonCommand);
            bindingSet.Bind(ValidateButton)
                      .For(v => v.Enabled)
                      .To(vm => vm.EnabledAction);
            bindingSet.Bind(ValidateButton)
                      .For("TitleColor")
                      .To(vm => vm.EnabledAction)
                      .WithConversion(new BoolToColorConverter(AppColorDefinition.White, AppColorDefinition.LightGray, c => c.ToUIColor()));
            bindingSet.Bind(ValidateButton)
                      .For(v => v.Hidden)
                      .To(vm => vm.ShowPrincipalButton)
                      .WithRevertedConversion();

            bindingSet.Bind(EditButton)
                      .For("Title")
                      .To(vm => vm.SecondaryButtonText);
            bindingSet.Bind(EditButton)
                      .To(vm => vm.SecondaryButtonCommand);
            bindingSet.Bind(EditButton)
                      .For(v => v.Hidden)
                      .To(vm => vm.ShowSecondaryButton)
                      .WithRevertedConversion();

            bindingSet.Bind(RemainingTime)
                      .To(vm => vm.RemainingTime);
            bindingSet.Bind(RemainingTime)
                      .For(v => v.Hidden)
                      .To(vm => vm.StillTimeToEdit)
                      .WithRevertedConversion();

            bindingSet.Apply();

            ItemInfosTableView.Source = source;
            ItemInfosTableView.ReloadData();
            ItemInfosTableView.TableFooterView = new UIView();

            if(ViewModel.HasRefreshingCapability)
            {
                ItemInfosTableView.RefreshControl = refreshingControl;
            }

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            if (!ViewModel.ShowSecondaryButton)
            {
                EditButton.RemoveFromSuperview();
                EditButton = null;
            }
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.ShowSecondaryButton))
            {
                if (!ViewModel.ShowSecondaryButton && EditButton != null)
                {
                    EditButton.RemoveFromSuperview();
                    EditButton = null;
                }
            }
        }
    }

    public class ProfileItemsTableViewSource : MvxTableViewSource
    {
        private static readonly NSString ItemInfoIdentifier = ProfileItemInfoViewCell.Key;
        private static readonly NSString ItemTitleIdentifier = ProfileItemInfoTitleViewCell.Key;

        public ProfileItemsTableViewSource(UITableView tableView)
            : base(tableView)
        {
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            tableView.RegisterNibForCellReuse(UINib.FromName(ItemInfoIdentifier, NSBundle.MainBundle),
                                              ItemInfoIdentifier);
            tableView.RegisterNibForCellReuse(UINib.FromName(ItemTitleIdentifier, NSBundle.MainBundle), ItemTitleIdentifier);
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath,
                                                              object item)
        {
            var vm = item as GenericInfoItemViewModel;
            if (vm.IsTitle)
            {
                return ViewFactory.Create<ProfileItemInfoTitleViewCell>();
            }
            else
            {
                return ViewFactory.Create<ProfileItemInfoViewCell>();
            }
        }
    }
}