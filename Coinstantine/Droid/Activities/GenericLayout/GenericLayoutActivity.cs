
using Android.App;
using Coinstantine.Core.ViewModels.Generic;
using Coinstantine.Core.ViewModels.ProfileValidation;
using Coinstantine.Droid.CustomViews.GenericLayout;
using MvvmCross.Binding.BindingContext;

namespace Coinstantine.Droid.Activities.GenericLayout
{
    [Activity(Label = "GenericLayoutActivity", Theme = "@style/AppTheme")]
    public class GenericLayoutActivity : BaseActivity<DisplayInfoGenericViewModel>
    {
        protected override bool WithSpecialBackground => true;
        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SetContentView(Resource.Layout.generic_layout_page);
            var genericView = FindViewById<GenericLayoutView>(Resource.Id.genericLayoutView);

            var bindingSet = this.CreateBindingSet<GenericLayoutActivity, DisplayInfoGenericViewModel>();

            bindingSet.Bind(genericView)
                      .For(v => v.DataContext)
                      .To(vm => vm.ActiveViewModel);

            bindingSet.Apply();
        }
    }
}
