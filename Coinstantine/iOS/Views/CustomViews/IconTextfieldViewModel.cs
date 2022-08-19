using System;
using System.Collections.Generic;
using System.Linq;
using Coinstantine.Core.ViewModels.Account.IconTextField;
using UIKit;

namespace Coinstantine.iOS
{
    public class IconTextfieldViewModel : UIPickerViewModel
    {
        private readonly IEnumerable<IconTextfieldItemsViewModel> _items;

        public IconTextfieldItemsViewModel SelectedItem { get; private set; }
        public event EventHandler ValueChanged;

        public IconTextfieldViewModel(IEnumerable<IconTextfieldItemsViewModel> items)
        {
            _items = items;
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return _items.Count();
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            return _items.ElementAt((int)row).Value;
        }

        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return 1;
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            SelectedItem = _items.ElementAt((int)row);
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}