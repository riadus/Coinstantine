using System.Threading.Tasks;
using CoreGraphics;
using Coinstantine.Common.Attributes;
using Coinstantine.Core.UIServices;
using Coinstantine.Core.ViewModels;
using Coinstantine.iOS.Views.Menu;
using UIKit;

namespace Coinstantine.iOS.UIServices
{
	[RegisterInterfaceAsLazySingleton]
	public class MenuManager : OverlayManager<FloatingMenuView>, IMenuManager
	{
		private UIImpactFeedbackGenerator _impact;

        protected override bool UseDefaultOverlay => false;

        public MenuManager()
		{
			TapToDismiss = false;
			Alpha = 0.8f;
			BeginInvokeOnMainThread(() =>
			{
				if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
				{
					_impact = new UIImpactFeedbackGenerator(UIImpactFeedbackStyle.Heavy);
					_impact.Prepare();
				}
			});
		}

		public Task HideMenu()
		{
			return RemoveView();
		}

		protected override Task RemoveView(bool animated = false)
		{
            if(View == null)
            {
                return Task.FromResult(0);
            }
			return View.Dismiss();
		}

		public Task ShowMenu(MenuViewModel context)
		{
			if (View?.Superview != null)
            {
                return Task.FromResult(0);
            }
			CreateViewIfNeeded();
			View.FromPoint = CGPoint.Empty;
			View.DataContext = context;

			return ShowView();
		}

		protected override void CreateViewIfNeeded()
		{
			if (View == null)
			{
				View = new FloatingMenuView
				{
					FromPoint = new CGPoint(0, 0)
				};
			}
		}

		public Task ShowMenuFrom(MenuViewModel context, TouchLocation touchLocation)
		{
			if (View?.Superview != null)
            {
                return Task.FromResult(0);
            }
			View = new FloatingMenuView(touchLocation.X, touchLocation.Y)
			{
				DataContext = context
			};

			_impact?.ImpactOccurred();

			return ShowView();
		}
	}
}
