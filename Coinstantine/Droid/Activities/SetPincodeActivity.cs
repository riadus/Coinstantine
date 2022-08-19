
using Android.App;
using Coinstantine.Core.ViewModels.Settings;
using Coinstantine.Droid.CustomViews;
using MvvmCross.Binding.BindingContext;

namespace Coinstantine.Droid.Activities
{
    [Activity(Label = "SetPincodeActivity", Theme = "@style/AppTheme")]
    public class SetPincodeActivity : BaseActivity<SetPincodeViewModel>
    {
        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.set_pincode);
            var pincodeView = FindViewById<PincodeView>(Resource.Id.setPincode_pincode);
            var bindingSet = this.CreateBindingSet<SetPincodeActivity, SetPincodeViewModel>();

            bindingSet.Bind(pincodeView)
                      .For(v => v.DataContext)
                      .To(vm => vm.PincodeViewModel);

            bindingSet.Apply();
        }

        public override void OnBackPressed()
        {
            if(ViewModel?.PincodeType == PincodeViewModel.PincodeType.ResetPinCode)
            {
                base.OnBackPressed();
            }
        }
    }
}
