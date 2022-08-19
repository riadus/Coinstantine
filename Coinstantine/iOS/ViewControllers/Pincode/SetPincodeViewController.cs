using System;
using Coinstantine.Core.ViewModels.Settings;
using Coinstantine.iOS.Views;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace Coinstantine.iOS.ViewControllers.Pincode
{
    public partial class SetPincodeViewController : BaseViewController<SetPincodeViewModel>
    {
        private PincodeView _pincodeView;
        public SetPincodeViewController() : base("SetPincodeViewController", null)
        {
            _pincodeView = ViewFactory.Create<PincodeView>();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NavigationController.NavigationBar.Hidden = !ViewModel.ShowNavigationBar;
            NavigationController.InteractivePopGestureRecognizer.Enabled = ViewModel.ShowNavigationBar;
        }

		protected override bool WithSpecialBackground => false;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var viewm = ViewModel;
            var topMargin = UIApplication.SharedApplication.StatusBarFrame.Height;
            if (!NavigationController.NavigationBar.Hidden)
            {
                topMargin += NavigationController.NavigationBar.Frame.Height;
            }
            _pincodeView.Frame = new CoreGraphics.CGRect(0, topMargin, View.Frame.Width, _pincodeView.Frame.Height - topMargin);
            Add(_pincodeView);
        }

        protected override void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<SetPincodeViewController, SetPincodeViewModel>();

            bindingSet.Bind(_pincodeView)
                      .For(v => v.DataContext)
                      .To(vm => vm.PincodeViewModel);

            bindingSet.Apply();
        }
    }
}

