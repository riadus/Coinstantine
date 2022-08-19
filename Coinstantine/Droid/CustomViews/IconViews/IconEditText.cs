using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Support.Constraints;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Coinstantine.Core;
using Coinstantine.Core.ViewModels.Account.IconTextField;
using Coinstantine.Droid.Extensions;

namespace Coinstantine.Droid.CustomViews.IconViews
{
    public class IconEditText : LinearLayout
    {
        private bool _isError;

        public IconEditText(Context context) : base(context)
        {
            CreateView();
        }

        public IconEditText(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            CreateView();
        }

        public Color SecondaryColor { get; set; }
        public Color MainColor { get; set; } = Color.White;
        public TextView Icon { get; set; }
        public Typeface IconTypeface { get; set; }
        public EditText EditText { get; set; }
        public View IconContainer { get; set; }
        public bool IsBigIcon { get; set; }
        public bool IsError { 
            get => _isError; 
            set
            {
                _isError = value;
                UpdateErrors();
            }
        }
        private IconTextFieldType _type;
        public IconTextFieldType Type
        {
            get => _type;
            set
            {
                _type = value;
                SetKeyboard();
            }
        }

        public object GrossHiddenValue { get; set; }
        public event EventHandler GrossHiddenValueChanged;

        private void SetKeyboard()
        {
            switch (Type)
            {
                case IconTextFieldType.Username:
                    EditText.InputType = InputTypes.TextVariationVisiblePassword | InputTypes.TextFlagNoSuggestions;
                    break;
                case IconTextFieldType.Email:
                    EditText.InputType = InputTypes.TextVariationEmailAddress;
                    break;
                case IconTextFieldType.Password:
                case IconTextFieldType.NewPassword:
                    EditText.InputType = InputTypes.TextVariationPassword | InputTypes.ClassText;
                    break;
                case IconTextFieldType.Date:
                    EditText.Focusable = false;
                    EditText.Clickable = true;
                    EditText.Click += EditText_Click;
                    break;
                case IconTextFieldType.List:
                    EditText.Focusable = false;
                    EditText.Clickable = true;
                    EditText.Click += EditText_Click;
                    break;
            }
        }

        protected override void OnDetachedFromWindow()
        {
            EditText.Click -= EditText_Click;
            EditText.FocusChange -= EditText_FocusChange;
            base.OnDetachedFromWindow();
        }

        void EditText_Click(object sender, EventArgs e)
        {
            if (Type == IconTextFieldType.Date)
            {
                OpenDatePicker();
            }
            if(Type == IconTextFieldType.List)
            {
                OpenCountryList();
            }
        }

        private void OpenDatePicker()
        {
            DatePickerFragment datePickerFragment = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                GrossHiddenValue = time;
                GrossHiddenValueChanged?.Invoke(this, EventArgs.Empty);
            });
            datePickerFragment.Show(Activity.FragmentManager, DatePickerFragment.TAG);
        }

        private void OpenCountryList()
        {
            CountryListPickerFragment countryListPickerFragment = CountryListPickerFragment.NewInstance(delegate (IconTextfieldItemsViewModel country)
            {
                GrossHiddenValue = country;
                GrossHiddenValueChanged?.Invoke(this, EventArgs.Empty);
            }, Items);
            countryListPickerFragment.Show(Activity.FragmentManager, CountryListPickerFragment.TAG);
        }

        public IEnumerable<IconTextfieldItemsViewModel> Items { get; set; }
        private Activity Activity => Context as Activity;

        internal void SetIcon()
        {
            Icon.SetTextColor(MainColor);
            IconContainer.SetBackgroundColor(SecondaryColor);
            CircleIcon();

            if (IsBigIcon)
            {
                Icon.SetTextColor(SecondaryColor);
                IconContainer.SetBackgroundColor(MainColor);
                BigIcon();
            }
        }

        public void CreateView()
        {
            BuildView();
        }

        public void StyleView()
        {
            EditText.SetTextColor(MainColor);
        }

        private void BuildView()
        {
            IconContainer = BuildIconContainer();
            Icon = BuildIcon();
            EditText = BuildEditText();
            EditText.FocusChange += EditText_FocusChange;
            var constraintLayout = new ConstraintLayout(Context)
            {
                LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
            };

            var size = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 30, Resources.DisplayMetrics);
            var iconContainerLayoutParams = new ConstraintLayout.LayoutParams(size, size)
            {
                BottomToBottom = ConstraintLayout.LayoutParams.ParentId,
                TopToTop = ConstraintLayout.LayoutParams.ParentId
            };
            constraintLayout.AddView(IconContainer, iconContainerLayoutParams);

            var iconLayoutParams = new ConstraintLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
            {
                BottomToBottom = ConstraintLayout.LayoutParams.ParentId,
                TopToTop = ConstraintLayout.LayoutParams.ParentId,
                LeftToLeft = IconContainer.Id,
                RightToRight = IconContainer.Id
            };

            constraintLayout.AddView(Icon, iconLayoutParams);

            AddView(constraintLayout);
            var editTextLayoutParams = new LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent)
            {
                LeftMargin = 10
            };
            AddView(EditText, editTextLayoutParams);
        }

        internal void CircleIcon()
        {
            IconContainer.SetBackgroundResource(Resource.Drawable.circle_view);
        }

        internal void BigIcon()
        {
            Icon.SetTextSize(ComplexUnitType.Dip, 30);
        }

        private View BuildIconContainer()
        {
            var iconContainer = new View(Context)
            {
                Id = GenerateViewId()
            };
            return iconContainer;
        }

        private TextView BuildIcon()
        {
            return new TextView(Context);
        }

        private EditText BuildEditText()
        {
            return new EditText(Context);
        }

        public event EventHandler OnFocus;
        public event EventHandler OnLostFocus;

        void EditText_FocusChange(object sender, FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {
                if (Type == IconTextFieldType.Date)
                {
                    OpenDatePicker();
                }
                else
                {
                    OnFocus?.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                OnLostFocus?.Invoke(this, EventArgs.Empty);
            }
        }

        private void UpdateErrors()
        {
            MainColor = (IsError ? AppColorDefinition.Red : AppColorDefinition.MainBlue).ToAndroidColor();
            SetIcon();
        }
    }
}
