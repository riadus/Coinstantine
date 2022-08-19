using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Coinstantine.Core.ViewModels;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace Coinstantine.iOS.Views.Menu
{
    public class CircleViews : MvxView
    {
        private readonly UIColor _color;
        private readonly Action dismissAction;
        private readonly List<CircleView> _views;

		private ObservableCollection<MenuItemViewModel> Items => DataContext as ObservableCollection<MenuItemViewModel>;

        public CircleViews(UIColor color, Action dismissAction)
        {
            _color = color;
            this.dismissAction = dismissAction;
            _views = new List<CircleView>();
        }

		private int DetermineNumberOfCircles(int numberOfItems) => (numberOfItems / 2) + 1 + (numberOfItems % 2);

        private void BuildView()
        {
			var numberOfCircles = DetermineNumberOfCircles(Items.Count);
			var smalestRadius =  (Frame.Size.Width / numberOfCircles) / 2;
			var smalestAlpha = 1.0f / (numberOfCircles + 1);
			var iconWidth = 30;
			var loops = 1;
			var items = 0;
            var rightLimit = 0d;

			for (var i = numberOfCircles - 1; i >= 0; i--)
			{
				var factor = ((i + 1) * 2) - 1;

                var cercle = new CircleView(smalestRadius * factor, ((float)factor) / 10)
                {
                    Center = Center,
                    BackgroundColor = _color,
                    Alpha = smalestAlpha * loops
                };
                if (loops == 1)
                {
                    rightLimit = cercle.Width;
                }
				if (i != 0)
				{
                    cercle.AddItemOnTop(new ItemView(iconWidth)
                    {
                        DataContext = Items[items],
                        RightLimit = rightLimit
					});
					items++;
					if (loops != 1 || Items.Count % 2 == 0)
					{
                        cercle.AddItemOnBottom(new ItemView(iconWidth)
                        {
                            DataContext = Items[items],
                            RightLimit = rightLimit
                        });
						items++;
					}
				}
				else
				{
					cercle.WithCloseOption = true;
					cercle.DismissAction = dismissAction;
				}
				loops++;
				_views.Add(cercle);
			}
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            BackgroundColor = UIColor.Clear;
			BuildView();
            foreach (var view in _views)
            {
                Add(view);
                view.LayoutSubviews();
            }
        }
        
        internal void Dismiss(Action onCompletedAction)
        {
            foreach (var view in _views)
            {
                if (view == _views.Last())
                {
                    view.Dismiss(() =>
                    {
                        RemoveFromSuperview();
                        onCompletedAction?.Invoke();
                    });
                }
                else
                {
                    view.Dismiss();
                }
            }
            _views.Clear();
        }
    }
}
