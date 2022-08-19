using Coinstantine.Core.ViewModels.Generic;
using Coinstantine.iOS.ViewControllers;
using MvvmCross.Binding.BindingContext;

namespace Coinstantine.iOS.Views.ProfileValidation
{
    public partial class DisplayInfoGenericViewController : BaseViewController<DisplayInfoGenericViewModel>
    {
        protected override bool WithSpecialBackground => true;

        private ProfileInfosView _profileInfosView;
        public DisplayInfoGenericViewController() : base("DisplayInfoGenericViewController", null)
        {
            _profileInfosView = ViewFactory.Create<ProfileInfosView>(); 
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _profileInfosView.Frame = new CoreGraphics.CGRect(0, 0, ContainerView.Frame.Width, ContainerView.Frame.Height);
            ContainerView.Add(_profileInfosView);
        }

        protected override void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<DisplayInfoGenericViewController, DisplayInfoGenericViewModel>();

            bindingSet.Bind(_profileInfosView)
                      .For(v => v.DataContext)
                      .To(vm => vm.ActiveViewModel);

            bindingSet.Apply();
        }
    }
}

