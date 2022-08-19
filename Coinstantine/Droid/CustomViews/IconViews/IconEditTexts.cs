using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Views;
using Android.Widget;
using Coinstantine.Core;
using Coinstantine.Core.ViewModels.Account.IconTextField;
using Coinstantine.Droid.CustomViews.BindableLayouts;
using Coinstantine.Droid.Extensions;
using MvvmCross.Binding.BindingContext;

namespace Coinstantine.Droid.CustomViews.IconViews
{
    public class IconEditTexts : BindableConstraintLayout
    {
        private List<IconEditText> _iconEditTexts;
        private const float Transparent = 0.0f;
        private const float NonTransparent = 1.0f;
        public IconEditTexts(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public IconEditTexts(Context context) : base(context)
        {
        }

        private IIconTextfieldCollectionViewModel ViewModel => DataContext as IIconTextfieldCollectionViewModel;

        protected override void OnAttachedToWindow()
        {
            ViewTreeObserver.GlobalLayout -= ViewTreeObserver_GlobalLayout;
            ViewTreeObserver.GlobalLayout += ViewTreeObserver_GlobalLayout;
            base.OnAttachedToWindow();
        }
        private bool _built;
        void ViewTreeObserver_GlobalLayout(object sender, System.EventArgs e)
        {
            if (_built)
            {
                return;
            }
            BuildView();
            _built = true;
        }

        protected override void OnDetachedFromWindow()
        {
            ViewTreeObserver.GlobalLayout -= ViewTreeObserver_GlobalLayout;
            base.OnDetachedFromWindow();
        }

        private void BuildView()
        {
            _iconEditTexts = new List<IconEditText>();
            var height = Height / (ViewModel.IconTextFields.Count() + 1);
            var margin = 30.ToDP(Context);
            var previousY = height;
            var bindingSet = this.CreateBindingSet<IconEditTexts, IIconTextfieldCollectionViewModel>();
            var index = 0;
            foreach (var iconTextfiledViewModel in ViewModel.IconTextFields)
            {
                var iconEditText = new IconEditText(Context)
                {
                    Id = index
                };
                var layoutParams = new LayoutParams(0, ViewGroup.LayoutParams.WrapContent)
                {
                    LeftMargin = margin,
                    TopMargin = previousY,
                    RightMargin = margin,
                    TopToTop = LayoutParams.ParentId,
                    RightToRight = LayoutParams.ParentId,
                    LeftToLeft = LayoutParams.ParentId,
                };
                iconEditText.StyleView();
                AnimateIconEditText(iconEditText);
                AddView(iconEditText, layoutParams);
                _iconEditTexts.Add(iconEditText);
                BindField(bindingSet, iconEditText, index);
                iconEditText.Measure(0, 0);
                previousY += iconEditText.MeasuredHeight + margin;
                index++;
            }

            var textview = new TextView(Context);
            var textviewLayoutParams = new LayoutParams(0, 0)
            {
                LeftMargin = margin,
                TopMargin = previousY + margin,
                RightMargin = margin,
                BottomMargin = margin,
                TopToTop = LayoutParams.ParentId,
                RightToRight = LayoutParams.ParentId,
                LeftToLeft = LayoutParams.ParentId,
                BottomToBottom = LayoutParams.ParentId
            };

            var button = new Button(Context)
            {
                Background = new ColorDrawable(AppColorDefinition.MainBlue.ToAndroidColor()),
            };
            button.SetTextColor(Color.White);
            var buttonLayoutParams = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
            {
                LeftMargin = margin,
                TopMargin = previousY + margin,
                RightMargin = margin,
                BottomMargin = margin,
                TopToTop = LayoutParams.ParentId,
                RightToRight = LayoutParams.ParentId,
                LeftToLeft = LayoutParams.ParentId,
            };

            bindingSet.Bind(button)
                      .To(vm => vm.ButtonCommand);
            bindingSet.Bind(button)
                       .For(v => v.Visibility)
                       .To(vm => vm.ButtonVisible)
                       .WithVisibilityGoneConversion();
            bindingSet.Bind(button)
                       .For(v => v.Text)
                       .To(vm => vm.ButtonText);

            bindingSet.Bind(textview)
                      .To(vm => vm.Error);

            AddView(textview, textviewLayoutParams);
            AddView(button, buttonLayoutParams);

            bindingSet.Apply();
        }

        private void BindField(MvxFluentBindingDescriptionSet<IconEditTexts, IIconTextfieldCollectionViewModel> bindingSet, IconEditText iconEditText, int index)
        {
            bindingSet.Bind(iconEditText.EditText)
                      .To(vm => vm.IconTextFields[index].Value);
            bindingSet.Bind(iconEditText)
                      .For("Icon")
                      .To(vm => vm.IconTextFields[index].Icon);
            bindingSet.Bind(iconEditText.EditText)
                      .For(v => v.Hint)
                      .To(vm => vm.IconTextFields[index].Placeholder);
            bindingSet.Bind(iconEditText)
                      .For(v => v.IsError)
                      .To(vm => vm.IconTextFields[index].IsError);
            bindingSet.Bind(iconEditText)
                      .For(v => v.Type)
                      .To(vm => vm.IconTextFields[index].Type);
            bindingSet.Bind(iconEditText)
                   .For("GrossHiddenValue")
                   .To(vm => vm.IconTextFields[index].GrossValue);
            bindingSet.Bind(iconEditText)
                 .For(v => v.Items)
                 .To(vm => vm.IconTextFields[index].Items);
        }

        private void AnimateIconEditText(IconEditText iconEditText)
        {
            var originalY = iconEditText.GetY();
            iconEditText.OnFocus += (sender, e) =>
            {
                foreach (var editText in _iconEditTexts)
                {
                    if (editText.Id != iconEditText.Id)
                    {
                        editText.Animate().Alpha(Transparent).SetDuration(400);
                        editText.Enabled = false;
                    }
                }
                var topY = _iconEditTexts[0].GetY();

                var y = topY - iconEditText.GetY();
                iconEditText.Animate().TranslationY(y).SetDuration(800);
            };
            iconEditText.OnLostFocus += (sender, e) =>
            {
                foreach (var textEdit in _iconEditTexts)
                {
                    textEdit.Animate().Alpha(NonTransparent).SetDuration(400);
                    textEdit.Enabled = true;
                }

                iconEditText.Animate().SetDuration(600).TranslationY(originalY);
            };
        }
    }
}
