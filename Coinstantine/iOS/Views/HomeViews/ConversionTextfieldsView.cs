using Coinstantine.Core;
using Coinstantine.iOS.Views.Extensions;
using System;
using System.Collections.Generic;
using UIKit;

namespace Coinstantine.iOS
{
    public partial class ConversionTextfieldsView : UIView
    {
        private Dictionary<UITextField, UILabel> _carets;
        public ConversionTextfieldsView (IntPtr handle) : base (handle)
        {
            _generalColor = AppColorDefinition.MainBlue.ToUIColor();
        }

        public UITextField AmountTextfield => Textfield1;
        public UITextField CostTextfield => Textfield2;

        public void Initialize()
        {
            Textfield1.EditingDidBegin += Textfield_EditingDidBegin;
            Textfield2.EditingDidBegin += Textfield_EditingDidBegin;

            Textfield1.EditingDidEnd += Textfield_EditingDidEnd;
            Textfield2.EditingDidEnd += Textfield_EditingDidEnd;

            _carets = new Dictionary<UITextField, UILabel>{
                {Textfield1, DownArrow},
                {Textfield2, UpArrow}
            };
            AddGestureRecognizer(new UITapGestureRecognizer(TapAction));
            Textfield1.ShouldReturn = (textField) => {
                textField.ResignFirstResponder();
                return false;
            };
            Textfield2.ShouldReturn = (textField) => {
                textField.ResignFirstResponder();
                return false;
            };
            ChangeFocus(Textfield1, false);
            ChangeFocus(Textfield2, false);

            UpArrow.Font = "caret-up".ToUIFont(20);
            UpArrow.Text = "caret-up";
            UpArrow.TextColor = AppColorDefinition.MainBlue.ToUIColor();

            DownArrow.Font = "caret-down".ToUIFont(20);
            DownArrow.Text = "caret-down";
            DownArrow.TextColor = AppColorDefinition.MainBlue.ToUIColor();
        }

        void Textfield_EditingDidBegin(object sender, EventArgs e)
        {
            ChangeFocus(sender as UITextField, true);
        }

        void Textfield_EditingDidEnd(object sender, EventArgs e)
        {
            ChangeFocus(sender as UITextField, false);
        }

        private void ChangeFocus(UITextField textField, bool hasFocus)
        {
            if(textField == null)
            {
                return;
            }
            if (!hasFocus)
            {
                textField.Layer.BorderWidth = 1;
                textField.Layer.BorderColor = GeneralColor.CGColor;
                _carets[textField].Alpha = 0.5f;
            }
            else
            {
                textField.Layer.BorderWidth = 3;
                textField.Layer.BorderColor = GeneralColor.CGColor;
                _carets[textField].Alpha = 1;
            }
        }
        UIColor _generalColor;

        public UIColor GeneralColor
        {
            get
            {
                return _generalColor;
            }

            set
            {
                _generalColor = value;
                Textfield1.Layer.BorderColor = _generalColor.CGColor;
                Textfield2.Layer.BorderColor = _generalColor.CGColor;
                UpArrow.TextColor = _generalColor;
                DownArrow.TextColor = _generalColor;
            }
        }

        void TapAction()
        {
            Textfield1.ResignFirstResponder();
            Textfield1.ResignFirstResponder();
        }
    }
}