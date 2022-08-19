using System;
using CoreGraphics;
using Coinstantine.Core.ViewModels.Home;
using Coinstantine.iOS.Views;
using MvvmCross.Binding.BindingContext;
using UIKit;
using MvvmCross.Platforms.Ios.Presenters.Attributes;

namespace Coinstantine.iOS.ViewControllers
{
    [MvxRootPresentation(WrapInNavigationController = true)]
    public partial class HomeViewController : BaseViewController<HomeViewModel>
    {
        private PrincipalView PrincipalView { get; set; }
        private BuyView BuyView { get; set; }

        public HomeViewController() : base("HomeViewController", null)
        {
            PrincipalView = ViewFactory.Create<PrincipalView>();
            BuyView = ViewFactory.Create<BuyView>();
        }
        private bool _built;
        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            if(_built){
                return;
            }
            var frame = View.Frame;
            var navBarHeight = NavBarHeight;
            ContentView.ParentViewController = this;
            PrincipalView.ParentViewController = this;
            PrincipalView.Frame = new CGRect(0, 0, frame.Width, frame.Height - navBarHeight);
            BuyView.Frame = new CGRect(0, PrincipalView.Frame.Y + PrincipalView.Frame.Height, frame.Width, PrincipalView.Frame.Height);
            ContentView.ContentSize = new CGSize(frame.Width, BuyView.Frame.Y + BuyView.Frame.Height);
            ContentView.Frame = new CGRect(0, navBarHeight, frame.Width, ContentView.Frame.Height);
            ContentView.Add(PrincipalView);
            ContentView.Add(BuyView);
            ScollBackToPincipal(false);
            _built = true;
        }

        protected override void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<HomeViewController, HomeViewModel>();

            bindingSet.Bind(PrincipalView)
                      .For(v => v.DataContext)
                      .To(vm => vm.PrincipalViewModel);
            bindingSet.Bind(BuyView)
                      .For(v => v.DataContext)
                      .To(vm => vm.BuyViewModel);
            bindingSet.Apply();

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

		protected override bool WithSpecialBackground => true;

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.ActiveViewModel))
            {
                if (ViewModel.ActiveViewModel == ViewModel.BuyViewModel)
                {
                    ScollToBuyCoinstantine();
                }
                else
                {
                    ScollBackToPincipal(true);
                    BuyView.EndEditing(true);
                }
            }
        }  

        private void ScollBackToPincipal(bool animated)
        {
            var action = new Action(() => {
                var offset = new CGPoint(0, animated ? -NavBarHeight : 0);
                ContentView.ContentOffset = offset;
            });
            if (animated)
            {
                UIView.Animate(1, action);
            }
            else
            {
                action();
            }
            
        }

        private void ScollToBuyCoinstantine()
        {
            UIView.Animate(0.8, () =>
            {
                var offset = new CGPoint(0, BuyView.Frame.Y - NavBarHeight);
                ContentView.ContentOffset = offset;
            });
        }
    }
}

