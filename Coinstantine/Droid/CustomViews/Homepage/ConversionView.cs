using System;
using System.Collections.Generic;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Support.Constraints;
using Android.Util;
using Android.Widget;
using Coinstantine.Core;
using Coinstantine.Droid.Extensions;

namespace Coinstantine.Droid.CustomViews.Homepage
{
    public class ConversionView : ConstraintLayout
    {
        public EditText ConversionEditText1 { get; set; }
        public EditText ConversionEditText2 { get; set; }
        public TextView CaretUpTextView { get; set; }
        public TextView CaretDownTextView { get; set; }

        private Dictionary<EditText, TextView> _carets;
        public ConversionView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize();
        }

        private void Initialize()
        {
            Inflate(Context, Resource.Layout.homepage_buyview_conversion_view, this);
            ConversionEditText1 = FindViewById<EditText>(Resource.Id.homepage_buyview_conversion1);
            ConversionEditText2 = FindViewById<EditText>(Resource.Id.homepage_buyview_conversion2);
            CaretUpTextView = FindViewById<TextView>(Resource.Id.homepage_buyview_conversion_caret_up);
            CaretDownTextView = FindViewById<TextView>(Resource.Id.homepage_buyview_conversion_caret_down);

            CaretUpTextView.Text = "caret-up";
            CaretUpTextView.Typeface = "caret-up".ToTypeface();
            CaretUpTextView.SetTextColor(AppColorDefinition.MainBlue.ToAndroidColor());
            CaretDownTextView.Text = "caret-down";
            CaretDownTextView.Typeface = "caret-down".ToTypeface();
            CaretDownTextView.SetTextColor(AppColorDefinition.MainBlue.ToAndroidColor());

            ConversionEditText1.FocusChange += EditText_FocusChange;
            ConversionEditText2.FocusChange += EditText_FocusChange;

            _carets = new Dictionary<EditText, TextView>{
                {ConversionEditText1, CaretDownTextView},
                {ConversionEditText2, CaretUpTextView}
            };
            ChangeFocus(ConversionEditText1, ConversionEditText1.HasFocus);
            ChangeFocus(ConversionEditText2, ConversionEditText2.HasFocus);
        }

        private Color _generalColor;

        public Color GeneralColor
        {
            get
            {
                return _generalColor;
            }

            set
            {
                _generalColor = value;
                ConversionEditText1.BackgroundTintList = ColorStateList.ValueOf(_generalColor);
                ConversionEditText2.BackgroundTintList = ColorStateList.ValueOf(_generalColor);
                CaretUpTextView.SetTextColor(_generalColor);
                CaretDownTextView.SetTextColor(_generalColor);
            }
        }

        void EditText_FocusChange(object sender, FocusChangeEventArgs e)
        {
            ChangeFocus(sender as EditText, e.HasFocus);
        }

        private void ChangeFocus(EditText textEdit, bool hasFocus)
        {
            if (textEdit == null)
            {
                return;
            }
            _carets[textEdit].Alpha = !hasFocus ? 0.5f : 1;
        }

        protected override void OnDetachedFromWindow()
        {
            ConversionEditText1.FocusChange -= EditText_FocusChange;
            ConversionEditText2.FocusChange -= EditText_FocusChange;
            base.OnDetachedFromWindow();
        }
    }
}
