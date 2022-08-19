using System;
using System.Threading.Tasks;
using CoreGraphics;
using Coinstantine.Core;
using Coinstantine.Core.ViewModels;
using Coinstantine.iOS.Views.Extensions;
using MvvmCross.Binding.BindingContext;
using UIKit;
using MvvmCross.Platforms.Ios.Binding.Views;

namespace Coinstantine.iOS.Views.Menu
{
	public class FloatingMenuView : MvxView
    {
		private readonly CircleViews _circleViews;
		public CGPoint FromPoint { get; set; }
		public FloatingMenuView(float x, float y)
        {
			this.DelayBind(SetBindings);
			_circleViews = new CircleViews(AppColorDefinition.MainBlue.ToUIColor(), async () => await Dismiss());
			FromPoint = new CGPoint(x, y);
        }

		public FloatingMenuView() : this(0, 0)
        {
        }
        
		internal async Task Dismiss()
		{
            if(_circleViews == null)
            {
                return;
            }
			await AnimateAsync(0.3,() =>
			{
				_circleViews.Dismiss(RemoveFromSuperview);
				Alpha = 0;
			});
            
		}

		private void SetBindings()
		{
			var bindingSet = this.CreateBindingSet<FloatingMenuView, MenuViewModel>();
			bindingSet.Bind(_circleViews)
					  .For(v => v.DataContext)
					  .To(vm => vm.Items);

			bindingSet.Apply();
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			BuildView();
			AddGestureRecognizer(new UITapGestureRecognizer(async () => await Dismiss())
			{
                Delegate = new GestureDelegate(typeof(CircleView))
			});
		}

        private void BuildView()
		{
			var min = Math.Min(Frame.Width, Frame.Height);
			_circleViews.Frame = new CGRect(FromPoint, new CGSize(min, min));
			_circleViews.Center = FromPoint;
			var deltaX = FromPoint != CGPoint.Empty ? 0 : 10;
			var deltaY = FromPoint != CGPoint.Empty ? 0 : 20;
			_circleViews.Alpha = 0;
			BackgroundColor = UIColor.Black;
			Alpha = 0.8f;
			Add(_circleViews);
			Animate(0.5f, 0, UIViewAnimationOptions.CurveEaseInOut, () =>
			{
				_circleViews.Alpha = 1;
				_circleViews.Center = new CGPoint(Center.X + deltaX, Center.Y + deltaY);
			}, () =>
			{
				Animate(0.5f, () => { _circleViews.Center = Center; });
			});
		}
	}
}
