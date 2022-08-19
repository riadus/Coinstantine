using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.Constraints;
using Android.Util;
using Android.Views;
using Android.Widget;
using Coinstantine.Core;
using Coinstantine.Core.ViewModels;
using Coinstantine.Droid.CustomViews.BindableLayouts;
using Coinstantine.Droid.Extensions;
using MvvmCross.Binding.BindingContext;

namespace Coinstantine.Droid.CustomViews
{
    public class MenuItemView : BindableConstraintLayout
    {
        public MenuItemView(Context context) : base(context)
        {
            BuildView();
        }

        public void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<MenuItemView, MenuItemViewModel>();

            bindingSet.Bind(_iconTextView)
                      .For("AppString")
                      .To(vm => vm.IconText);
            bindingSet.Bind(_iconTextView)
                      .For("Disabled")
                      .To(vm => vm.IsEnabled);

            bindingSet.Bind(_titleTextView)
                      .To(vm => vm.Text);
            bindingSet.Bind(_titleTextView)
                      .For("Disabled")
                      .To(vm => vm.IsEnabled);

            bindingSet.Bind(_button)
                      .To(vm => vm.SelectionCommand);

            bindingSet.Apply();
        }

        private TextView _iconTextView { get; set; }
        private TextView _titleTextView { get; set; }
        private Button _button { get; set; }
        private View _iconContainer { get; set; }

        public void CreateView()
        {
            BuildView();
            SetClipChildren(false);
            Background = new ColorDrawable(Color.Blue);
        }

        private void BuildView()
        {
            _iconContainer = BuildIconContainer();
            _iconTextView = BuildIcon();
            _button = BuildButton();
            CircleIcon();
            _titleTextView = BuildTitle();
            var constraintLayout = new ConstraintLayout(Context)
            {
                LayoutParameters = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
                {
                    BottomToBottom = LayoutParams.ParentId,
                    TopToTop = LayoutParams.ParentId
                },
                Id = GenerateViewId()
            };

            var size = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 30, Resources.DisplayMetrics);
            var iconContainerLayoutParams = new LayoutParams(size, size)
            {
                BottomToBottom = LayoutParams.ParentId,
                TopToTop = LayoutParams.ParentId
            };
            constraintLayout.AddView(_iconContainer, iconContainerLayoutParams);

            var iconLayoutParams = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
            {
                BottomToBottom = LayoutParams.ParentId,
                TopToTop = LayoutParams.ParentId,
                LeftToLeft = _iconContainer.Id,
                RightToRight = _iconContainer.Id
            };

            constraintLayout.AddView(_iconTextView, iconLayoutParams);

            AddView(constraintLayout);
            var editTextLayoutParams = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
            {
                LeftMargin = 30,
                TopToTop = LayoutParams.ParentId,
                BottomToBottom = LayoutParams.ParentId,
                LeftToRight = constraintLayout.Id
            };
            AddView(_titleTextView, editTextLayoutParams);

            var buttonLayoutParams = new LayoutParams(0, ViewGroup.LayoutParams.WrapContent)
            {
                LeftToLeft = LayoutParams.ParentId,
                RightToRight = LayoutParams.ParentId,
            };

            AddView(_button, buttonLayoutParams);
        }

        internal void CircleIcon()
        {
            _iconContainer.SetBackgroundResource(Resource.Drawable.circle_view);
        }

        internal void BigIcon()
        {
            _iconTextView.SetTextSize(ComplexUnitType.Dip, 30);
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
            var textView = new TextView(Context);
            textView.SetTextColor(AppColorDefinition.MainBlue.ToAndroidColor());
            return textView;
        }

        private TextView BuildTitle()
        {
            var textView = new TextView(Context);
            textView.SetTextColor(Color.White);
            textView.Typeface = Typeface.DefaultBold;
            return textView;
        }

        private Button BuildButton()
        {
            return new Button(Context)
            {
                Background = new ColorDrawable(Color.Transparent)
            };
        }
    }
}
