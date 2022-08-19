using System;
using Android.Content;
using Android.Widget;
using Coinstantine.Core.ViewModels.Generic;
using Coinstantine.Core.ViewModels.ProfileValidation;
using Coinstantine.Droid.CustomViews.BindableLayouts;
using MvvmCross.Binding.BindingContext;

namespace Coinstantine.Droid.CustomViews.GenericLayout
{
    public class GenericLayoutTitleItem : BindableConstraintLayout
    {
        protected TextView Title { get; set; }
        protected TextView Value { get; set; }

        public GenericLayoutTitleItem(Context context) : base(context)
        {
            this.DelayBind(SetBindings);
            Initialize();
        }

        private void Initialize()
        {
            Inflate(Context, Resource.Layout.generic_layout_title_item, this);
            Title = FindViewById<TextView>(Resource.Id.genericLayoutTitleItemTitle);
            Value = FindViewById<TextView>(Resource.Id.genericLayoutTitleItemValue);
        }

        private void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<GenericLayoutTitleItem, GenericInfoItemViewModel>();

            bindingSet.Bind(Title)
                      .To(vm => vm.Title1);
            bindingSet.Bind(Value)
                      .To(vm => vm.Value1);

            bindingSet.Apply();
        }
    }
}
