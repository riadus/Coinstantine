using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coinstantine.Core.ViewModels.Settings;
using Coinstantine.iOS.ViewControllers;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace Coinstantine.iOS.Views.Settings
{
    public partial class ChangeLanguageViewController : BaseViewController<ChangeLanguageViewModel>
    {
        protected override bool WithSpecialBackground => true;

        public ChangeLanguageViewController() : base("ChangeLanguageViewController", null)
        {
        }
        private SortableTableViewSource _source;
        protected override void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<ChangeLanguageViewController, ChangeLanguageViewModel>();

            _source = new SortableTableViewSource(LanguagesTableView, LanguageItemViewCell.Key, LanguageItemViewCell.Key);
           
            bindingSet.Bind(_source)
                      .To(vm => vm.Languages);
            
            bindingSet.Bind(_source)
                      .For(s => s.SelectedItem)
                      .To(vm => vm.SelectedLanguage);
            
            bindingSet.Apply();
            _source.Reordred += Source_Reordred;
            LanguagesTableView.Source = _source;
            LanguagesTableView.ReloadData();
            LanguagesTableView.TableFooterView = new UIView();
        }

        void Source_Reordred(object sender, EventArgs e)
        {
            ViewModel.ReorderList();
        }

        public override void ViewDidUnload()
        {
            _source.Reordred -= Source_Reordred;
            base.ViewDidUnload();
        }
    }

    public class SortableTableViewSource : MvxSimpleTableViewSource
    {
        public SortableTableViewSource(IntPtr handle) : base(handle)
        {
        }

        public SortableTableViewSource(UITableView tableView, Type cellType, string cellIdentifier = null) : base(tableView, cellType, cellIdentifier)
        {
        }

        public SortableTableViewSource(UITableView tableView, string nibName, string cellIdentifier = null, NSBundle bundle = null) : base(tableView, nibName, cellIdentifier, bundle)
        {
            SelectedItemChanged += SortableTableViewSource_SelectedItemChanged;
        }
       
        LanguageItemViewModel _selectedItem;
        LanguageItemViewModel _previousSelection;

        public event EventHandler Reordred;

        private async Task Reorder()
        {
            var source = (ItemsSource as IEnumerable<LanguageItemViewModel>)?.ToList();
            var index = source.IndexOf(_selectedItem);
            await Task.Delay(300);
            TableView.BeginUpdates();
            TableView.MoveRow(NSIndexPath.Create(new[] { 0, index }), NSIndexPath.Create(new[] { 0, 0 }));
            TableView.EndUpdates();
            Reordred?.Invoke(this, EventArgs.Empty);
        }

        async void SortableTableViewSource_SelectedItemChanged(object sender, EventArgs e)
        {
            _selectedItem = SelectedItem as LanguageItemViewModel;
            if (_previousSelection != null)
            {
                await Reorder();
            }
            _previousSelection = _selectedItem;
        }
    }
}

