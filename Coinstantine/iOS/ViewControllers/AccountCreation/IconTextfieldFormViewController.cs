using Coinstantine.Core.ViewModels.Account.AccountCreation;
using Coinstantine.Core.ViewModels.Account.IconTextField;
using Coinstantine.Core.ViewModels.Account.Login;
using Coinstantine.iOS.Views.Extensions;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace Coinstantine.iOS.ViewControllers.AccountCreation
{
    public class CreateAccountViewController : IconTextfieldFormViewController<CreateAccountViewModel>
    { }

    public class LoginViewController : IconTextfieldFormViewController<LoginViewModel>
    { }

    public partial class IconTextfieldFormViewController<T> : BaseViewController<T> where T : class, IIconTextfieldFormViewModel
    {
        protected override bool WithSpecialBackground => false;

        public IconTextfieldFormViewController() : base("IconTextfieldFormViewController", null)
        {
        }

        protected override void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<IconTextfieldFormViewController<T>, IIconTextfieldFormViewModel>();

            bindingSet.Bind(ScrollView)
                    .For(s => s.DataContext)
                    .To(vm => vm);
            bindingSet.Bind(PageControl)
                    .For(v => v.Hidden)
                    .To(vm => vm.IsMultiPage)
                    .WithRevertedConversion();
            bindingSet.Bind(PageControl)
                    .For(p => p.Pages)
                    .To(vm => vm.Count);
            bindingSet.Bind(NextButton)
                    .For("Title")
                    .To(vm => vm.ButtonText);  
            bindingSet.Bind(NextButton)
                    .To(vm => vm.ButtonCommand);
            bindingSet.Bind(PageControl)
                    .For(p => p.CurrentPage)
                    .To(vm => vm.CurrentIndex);
            bindingSet.Bind(ScrollView)
                    .For(s => s.CurrentView)
                    .To(vm => vm.CurrentIndex);
            bindingSet.Bind(ActivityIndicator)
                    .For(v => v.Hidden)
                    .To(vm => vm.IsLoading)
                    .WithRevertedConversion();
            bindingSet.Apply();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var tapGesture = new UITapGestureRecognizer(HandleAction)
            {
                CancelsTouchesInView = false
            };
            ScrollView.ParentViewController = this;
            ScrollView.AddGestureRecognizer(tapGesture);
            ScrollView.BackgroundColor = MainBlueColor;
            PageControl.UserInteractionEnabled = false;
            NextButton.ToRegularStyle();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        void HandleAction()
        {
            ScrollView.Release();
        }
    }
}
