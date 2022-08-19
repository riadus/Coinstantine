using System;
using Android.Content;
using Android.Widget;
using Coinstantine.Core.ViewModels.Generic;
using Coinstantine.Core.ViewModels.ProfileValidation;
using Coinstantine.Droid.CustomViews.BindableLayouts;
using Coinstantine.Droid.Extensions;
using MvvmCross.Binding.BindingContext;

namespace Coinstantine.Droid.CustomViews.GenericLayout
{
    public class GenericLayoutItem : BindableConstraintLayout
    {
        protected TextView Title1 { get; set; }
        protected TextView Value1 { get; set; }
        protected TextView Title2 { get; set; }
        protected TextView Value2 { get; set; }
        protected TextView Value3 { get; set; }

        public GenericLayoutItem(Context context) : base(context)
        {
            this.DelayBind(SetBindings);
            Initialize();
        }

        private void Initialize()
        {
            Inflate(Context, Resource.Layout.generic_layout_item, this);
            Title1 = FindViewById<TextView>(Resource.Id.genericLayoutItemTitle1);
            Value1 = FindViewById<TextView>(Resource.Id.genericLayoutItemValue1);
            Title2 = FindViewById<TextView>(Resource.Id.genericLayoutItemTitle2);
            Value2 = FindViewById<TextView>(Resource.Id.genericLayoutItemValue2);
            Value3 = FindViewById<TextView>(Resource.Id.genericLayoutItemValue3);
        }

        private void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<GenericLayoutItem, GenericInfoItemViewModel>();

            bindingSet.Bind(Title1)
                      .To(vm => vm.Title1);
            bindingSet.Bind(Value1)
                      .To(vm => vm.Value1);

            bindingSet.Bind(Title2)
                      .To(vm => vm.Title2);
            bindingSet.Bind(Title2)
                      .For(v => v.Visibility)
                      .To(vm => vm.ShowSecondTitle)
                      .WithVisibilityGoneConversion();
            bindingSet.Bind(Value2)
                      .To(vm => vm.Value2);
            bindingSet.Bind(Value2)
                      .For(v => v.Visibility)
                      .To(vm => vm.ShowSecondValue)
                      .WithVisibilityGoneConversion();

            bindingSet.Bind(Value3)
                      .For("AppString")
                      .To(vm => vm.Value3);
            bindingSet.Bind(Value3)
                      .For(v => v.Visibility)
                      .To(vm => vm.ShowThirdValue)
                      .WithVisibilityGoneConversion();

            bindingSet.Apply();
        }
    }
}
