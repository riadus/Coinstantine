using System;
using System.Collections.Generic;
using Coinstantine.Core.ViewModels.Account.IconTextField;
using CoreGraphics;
using UIKit;

namespace Coinstantine.iOS
{
    public partial class IconTextField : AutosizeTextField
    {
        public IconTextField(IntPtr handle) : base(handle)
        {
            CircleAppStringLabel = new CircleAppStringLabel();
        }

        public IconTextField()
        {
            CircleAppStringLabel = new CircleAppStringLabel();
        }

        public CircleAppStringLabel CircleAppStringLabel { get; }
        private bool _build;
        private IconTextFieldType _type;
        public object GrossHiddenValue { get; set; }
        public event EventHandler GrossHiddenValueChanged;
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            if (_build)
            {
                return;
            }
            CircleAppStringLabel.Frame = new CGRect(Frame.Height / 4, Frame.Height / 4, Frame.Height / 2, Frame.Height / 2);
            CircleAppStringLabel.Layer.CornerRadius = Frame.Height / 4;
            LeftView = new UIView(new CGRect(0, 0, Frame.Height, Frame.Height));
            LeftViewMode = UITextFieldViewMode.Always;
            Add(CircleAppStringLabel);
            _build = true;
        }

        public IconTextFieldType Type
        {
            get => _type;
            set
            {
                _type = value;
                SetKeyboard();
            }
        }

        private void SetKeyboard()
        {
            switch (Type)
            {
                case IconTextFieldType.Username:
                    if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                    {
                        TextContentType = UITextContentType.Username;
                    }
                    break;
                case IconTextFieldType.Email:
                    TextContentType = UITextContentType.EmailAddress;
                    KeyboardType = UIKeyboardType.EmailAddress;
                    break;
                case IconTextFieldType.Password:
                    if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                    {
                        TextContentType = UITextContentType.Password;
                    }
                    SecureTextEntry = true;
                    break;
                case IconTextFieldType.NewPassword:
                    if (UIDevice.CurrentDevice.CheckSystemVersion(12, 0))
                    {
                        TextContentType = UITextContentType.NewPassword;
                    }
                    SecureTextEntry = true;
                    break;
                case IconTextFieldType.Date:
                    AddDatePicker();
                    break;
            }
        }
        private UIDatePicker _datePicker;
        private IconTextfieldViewModel _model;
        private IEnumerable<IconTextfieldItemsViewModel> _items;
        private bool _isError;

        private void AddDatePicker()
        {
            _datePicker = new UIDatePicker
            {
                Mode = UIDatePickerMode.Date
            };
            _datePicker.ValueChanged -= DatePicker_ValueChanged;
            _datePicker.ValueChanged += DatePicker_ValueChanged;
            InputView = _datePicker;
        }

        void DatePicker_ValueChanged(object sender, EventArgs e)
        {
            GrossHiddenValue = (DateTime)_datePicker.Date;
            GrossHiddenValueChanged?.Invoke(this, EventArgs.Empty);
        }

        public IEnumerable<IconTextfieldItemsViewModel> Items
        {
            get => _items;
            set
            {
                if (value == null)
                {
                    return;
                }
                _items = value;
                PopulateList();
            }
        }

        private void PopulateList()
        {
            _model = new IconTextfieldViewModel(Items);
            _model.ValueChanged -= Model_ValueChanged;
            _model.ValueChanged += Model_ValueChanged;
            InputView = new UIPickerView
            {
                Model = _model
            };
        }

        void Model_ValueChanged(object sender, EventArgs e)
        {
            GrossHiddenValue = _model.SelectedItem;
            GrossHiddenValueChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool IsError
        {
            get => _isError; 
            set
            {
                _isError = value;
                UpdateErrors();
            }
        }

        private void UpdateErrors()
        {
            CircleAppStringLabel.ChangeStatus(IsError);
        }
    }
}