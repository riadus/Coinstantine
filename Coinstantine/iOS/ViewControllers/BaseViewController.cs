using System;
using Coinstantine.Common;
using Coinstantine.Core;
using Coinstantine.Core.ViewModels;
using Coinstantine.iOS.Views.Base;
using Coinstantine.iOS.Views.Extensions;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace Coinstantine.iOS.ViewControllers
{
    public class BaseView : MvxView
    {
        public BaseView(IntPtr handle) : base(handle)
        {
            
        }

        protected UIColor MainBlueColor => AppColorDefinition.MainBlue.ToUIColor();
        protected UIColor SecondaryColor => AppColorDefinition.SecondaryColor.ToUIColor();
    }

    public abstract class BaseViewController<TViewModel> : MvxViewController<TViewModel> where TViewModel : class, IBaseViewModel
    {
		 protected nfloat NavBarHeight
        {
            get
            {
                var navBar = NavigationController?.NavigationBar?.Hidden ?? true ? 0 : NavigationController.NavigationBar.Frame.Height;
                return navBar + UIApplication.SharedApplication.StatusBarFrame.Height;
            }
        }
        protected BaseViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
            this.DelayBind(SetBaseBindings);
        }

		protected abstract bool WithSpecialBackground { get; }

        private string _specialBackgroundIconTitle { get; set; }
        private string _specialBackgroundTitle { get; set; }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            if (ViewModel.HasMenu)
            {
                BuildNavigationBarItems();
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.BackgroundColor = MainBlueColor;
        }
        private BackgroundBubbles _backgroundCircles;
        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            AddBubbles();
        }

        private void AddBubbles()
        {
            if (WithSpecialBackground)
            {
                _backgroundCircles?.RemoveFromSuperview();

                _backgroundCircles = new BackgroundBubbles(new CGRect(0, 0, View.Frame.Width, View.Frame.Width))
                {
                    BubbleCenter = new CGPoint(50, NavBarHeight + 40),
                    TitleIcon = _specialBackgroundIconTitle,
                    Title = _specialBackgroundTitle
                };
                _backgroundCircles.ClipsToBounds = true;
                Add(_backgroundCircles);
                View.SendSubviewToBack(_backgroundCircles);
            }
        }

        private void SetBaseBindings()
        {
            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            SetTitlesFromViewModel();
            SetBindings();
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.Title))
            {
                SetTitlesFromViewModel();
                AddBubbles();
            }
        }

        private void SetTitlesFromViewModel()
        {
            var dataContext = ViewModel as IBaseViewModel;
            if (dataContext?.Title != null)
            {
                _specialBackgroundTitle = dataContext.GetTitle();
            }
            if (dataContext?.TitleIcon.IsNotNull() ?? false)
            {
                _specialBackgroundIconTitle = dataContext.TitleIcon;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (ViewModel != null)
            {
                ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            }
            base.Dispose(disposing);
        }

        protected abstract void SetBindings();

        public bool TextFieldShouldReturn(UITextField textfield)
        {
            nint nextTag = textfield.Tag + 1;
            UIResponder nextResponder = this.View.ViewWithTag(nextTag);
            if (nextResponder != null)
            {
                nextResponder.BecomeFirstResponder();
            }
            else
            {
                textfield.ResignFirstResponder();
                OnResignFirstResponder?.Invoke(this, EventArgs.Empty);
            }
            return false;
        }

        public event EventHandler<EventArgs> OnResignFirstResponder;

        public override bool CanBecomeFirstResponder => true;

        
        protected void BuildNavigationBarItems()
		{
            var menuIcon = ViewModel.MenuIcon.ToCode();
            var str = $"{menuIcon} {ViewModel.MenuText}";
            var faTextAttribute = new UIStringAttributes
            {
                Font = UIFont.FromName("FontAwesome5Free-Solid", 25),
                ForegroundColor = MainBlueColor
            };

            var normalTextAttribute = new UIStringAttributes
            {
                Font = UIFont.SystemFontOfSize(20),
                ForegroundColor = MainBlueColor
            };

            var prettyString = new NSMutableAttributedString(str);
            // Coloring the placeholder
            prettyString.SetAttributes(faTextAttribute.Dictionary, new NSRange(0, 1));
            prettyString.SetAttributes(normalTextAttribute.Dictionary, new NSRange(1, str.Length - 1));

            var menuButton = new UIButton();
            menuButton.SetAttributedTitle(prettyString, UIControlState.Normal);
            menuButton.VerticalAlignment = UIControlContentVerticalAlignment.Center;
            menuButton.TouchUpInside += Menu_TouchUpInside;

            var syncButton = new UIButton();
            syncButton.SetTitle(ViewModel.SyncIcon.ToCode(), UIControlState.Normal);
            syncButton.SetTitleColor(MainBlueColor, UIControlState.Normal);
            syncButton.TitleLabel.Font = ViewModel.SyncIcon.ToUIFont(20);
            syncButton.VerticalAlignment = UIControlContentVerticalAlignment.Center;
            syncButton.TouchUpInside += Sync_TouchUpInside;
             
            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(menuButton);
            NavigationItem.RightBarButtonItem = new UIBarButtonItem(syncButton);
		}

        void Menu_TouchUpInside(object sender, EventArgs e)
        {
            ViewModel.OpenMenu.Execute();
        }

        void Sync_TouchUpInside(object sender, EventArgs e)
        {
            ViewModel.SyncCommand.Execute();
        }


        public override void TouchesMoved(NSSet touches, UIEvent evt)
		{
			base.TouchesMoved(touches, evt);
			if (touches.AnyObject is UITouch touch)
			{
				var force = touch.Force;
                if(force == touch.MaximumPossibleForce && force > 0)
				{
					var location = touch.GetPreciseLocation(View);
					ViewModel.OpenMenuFrom.Execute(new TouchLocation((float) location.X, (float)location.Y));
				}
			}
		}

        protected UIColor MainBlueColor => AppColorDefinition.MainBlue.ToUIColor();
        protected UIColor SecondaryColor => AppColorDefinition.SecondaryColor.ToUIColor();
	}
}
