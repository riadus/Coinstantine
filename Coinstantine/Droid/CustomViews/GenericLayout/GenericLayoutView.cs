using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Coinstantine.Core;
using Coinstantine.Core.Converters;
using Coinstantine.Core.ViewModels.Generic;
using Coinstantine.Core.ViewModels.ProfileValidation;
using Coinstantine.Droid.CustomViews.BindableLayouts;
using Coinstantine.Droid.Extensions;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;

namespace Coinstantine.Droid.CustomViews.GenericLayout
{
    public class GenericLayoutView : BindableConstraintLayout
    {
        protected TextView Title { get; set; }
        protected MvxListView List { get; set; }
        protected Button EditButton { get; set; }
        protected Button ValidateButton { get; set; }
        protected TextView RemainingTime { get; set; }
        protected MvxSwipeRefreshLayout SwipeRefreshLayout { get; set; }

        public GenericLayoutView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            this.DelayBind(SetBindings);
            Initialize();
        }

        private void Initialize()
        {
            Inflate(Context, Resource.Layout.generic_layout_view, this);
            Title = FindViewById<TextView>(Resource.Id.genericLayoutViewTitle);
            SwipeRefreshLayout = FindViewById<MvxSwipeRefreshLayout>(Resource.Id.genericLayoutViewListRefreshLayout);
            List = FindViewById<MvxListView>(Resource.Id.genericLayoutViewList);
            EditButton = FindViewById<Button>(Resource.Id.genericLayoutViewEditButton);
            ValidateButton = FindViewById<Button>(Resource.Id.genericLayoutViewValidateButton);
            RemainingTime = FindViewById<TextView>(Resource.Id.genericLayoutViewRemainingTime);
            ValidateButton.SetBackgroundResource(Resource.Drawable.black_button);
        }

        private void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<GenericLayoutView, IGenericInfoViewModel>();
            List.Adapter = new GenericLayoutViewAdapter(Context, BindingContext);
            bindingSet.Bind(Title)
                      .To(vm => vm.InfoTitle);

            bindingSet.Bind(SwipeRefreshLayout)
                      .For(v => v.Refreshing)
                      .To(vm => vm.IsRefreshing);
            bindingSet.Bind(SwipeRefreshLayout)
                      .For(v => v.RefreshCommand)
                      .To(vm => vm.RefreshCommand);
            bindingSet.Bind(SwipeRefreshLayout)
                .For(v => v.Enabled)
                .To(vm => vm.HasRefreshingCapability);

            bindingSet.Bind(List)
                      .For(vm => vm.ItemsSource)
                      .To(vm => vm.GenericInfoItems);

            bindingSet.Bind(ValidateButton)
                      .For(v => v.Text)
                      .To(vm => vm.PrincipalButtonText);
            bindingSet.Bind(ValidateButton)
                      .To(vm => vm.PrincipalButtonCommand);
            bindingSet.Bind(ValidateButton)
                      .For(v => v.Enabled)
                      .To(vm => vm.EnabledAction);
            bindingSet.Bind(ValidateButton)
                      .For("TextColor")
                      .To(vm => vm.EnabledAction)
                      .WithConversion(new BoolToColorConverter(AppColorDefinition.White, AppColorDefinition.LightGray, c => c.ToAndroidColor()));
            bindingSet.Bind(ValidateButton)
                      .For(v => v.Visibility)
                      .To(vm => vm.ShowPrincipalButton)
                      .WithVisibilityConversion();

            bindingSet.Bind(EditButton)
                      .For(v => v.Text)
                      .To(vm => vm.SecondaryButtonText);
            bindingSet.Bind(EditButton)
                      .To(vm => vm.SecondaryButtonCommand);
            bindingSet.Bind(EditButton)
                      .For(v => v.Visibility)
                      .To(vm => vm.ShowSecondaryButton)
                      .WithVisibilityGoneConversion();

            bindingSet.Bind(RemainingTime)
                      .To(vm => vm.RemainingTime);
            bindingSet.Bind(RemainingTime)
                      .For(v => v.Visibility)
                      .To(vm => vm.StillTimeToEdit)
                      .WithVisibilityGoneConversion();

            bindingSet.Apply();
        }
    }

    public class GenericLayoutViewAdapter : MvxAdapter 
    {
        public GenericLayoutViewAdapter(Context context, IMvxBindingContext bindingContext) : base(context, (IMvxAndroidBindingContext)bindingContext)
        {
        }

        protected override View GetBindableView(View convertView, object dataContext, ViewGroup parent, int templateId)
        {
            var vm = dataContext as GenericInfoItemViewModel;
            return vm.IsTitle
                ? new GenericLayoutTitleItem(Context)
                {
                    DataContext = dataContext
                }
                : (View)new GenericLayoutItem(Context)
                {
                    DataContext = dataContext
                };
        }

    }

}
