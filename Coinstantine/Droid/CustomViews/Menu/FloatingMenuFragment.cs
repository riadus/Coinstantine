using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.OS;
using Android.Support.Constraints;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Coinstantine.Core;
using Coinstantine.Core.ViewModels;
using Coinstantine.Droid.Extensions;

namespace Coinstantine.Droid.CustomViews.Menu
{
    public class FloatingMenuFragment : DialogFragment
    {
        private readonly Activity _activity;
        private readonly MenuViewModel _menuViewModel;
        private readonly Func<Task> _dismissTask;

        public FloatingMenuFragment(Activity activity, MenuViewModel menuViewModel, Func<Task> dismissTask)
        {
            _activity = activity;
            _menuViewModel = menuViewModel;
            _dismissTask = dismissTask;
        }

        public override void OnStart()
        {
            base.OnStart();
            if (Dialog != null)
            {
                var backgroundColor = new ColorDrawable(Color.Black);
                backgroundColor.SetAlpha(0);
                Dialog.Window.SetBackgroundDrawable(backgroundColor);
                Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var layout = new CircleViewsLayout(Context, _activity, _dismissTask);
            layout.Build(_menuViewModel);

            Dialog.Window.Attributes.WindowAnimations = Resource.Style.DialogAnimation;
            return layout;
        }
    }

    public class CircleViewsLayout : ConstraintLayout
    {
        private readonly Activity _activity;
        private readonly Func<Task> _dismissTask;
        private MenuViewModel _dataContext;
        private ObservableCollection<MenuItemViewModel> Items => _dataContext.Items;
        public CircleViewsLayout(Context context, Activity activity, Func<Task> dismissTask) : base(context)
        {
            _activity = activity;
            _dismissTask = dismissTask;
        }

        public void Build(MenuViewModel dataContext)
        {
            _dataContext = dataContext;
            BuildLayout();
        }

        private int DetermineNumberOfCircles(int numberOfItems) => (numberOfItems / 2) + 1 + (numberOfItems % 2);

        private void BuildLayout()
        {
            LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            SetClipChildren(false);
            BuildCircleViews();
            Click += CircleViewsLayout_Click;
        }

        private void BuildCircleViews()
        {
            var metrics = new DisplayMetrics();
            _activity.WindowManager.DefaultDisplay.GetMetrics(metrics);

            var numberOfCircles = DetermineNumberOfCircles(Items.Count);
            var smalestRadius = (metrics.WidthPixels / numberOfCircles) / 2;
            var smalestAlpha = 255f / (numberOfCircles + 1);
            var loops = 1;
            var items = 0;
            for (var i = numberOfCircles - 1; i >= 0; i--)
            {
                var factor = ((i + 1) * 2) - 1;
                var width = smalestRadius * factor;
                var alpha = smalestAlpha * loops;
                var viewLayoutParameters = new LayoutParams(width, width)
                {
                    TopToTop = LayoutParams.ParentId,
                    BottomToBottom = LayoutParams.ParentId,
                    LeftToLeft = LayoutParams.ParentId,
                    RightToRight = LayoutParams.ParentId
                };
                var circle = new CircleView(Context);

                if (i != 0)
                {
                    circle.AddItemOnTop(new MenuItemView(Context)
                    {
                        DataContext = Items[items]
                    });
                    items++;
                    if (loops != 1 || Items.Count % 2 == 0)
                    {
                        circle.AddItemOnBottom(new MenuItemView(Context)
                        {
                            DataContext = Items[items]
                        });
                        items++;
                    }
                }
                else
                {
                    circle.WithCloseOption = true;
                    circle.DismissTask = _dismissTask;
                }
                circle.BuildView(width, (int)alpha);
                AddView(circle, viewLayoutParameters);
                loops++;
            }
        }

        async void CircleViewsLayout_Click(object sender, EventArgs e)
        {
            Click -= CircleViewsLayout_Click;
            await _dismissTask().ConfigureAwait(false);
        }

        public class CircleView : ConstraintLayout
        {
            private MenuItemView _topItem;
            private MenuItemView _bottomItem;

            public bool WithCloseOption { get; set; }
            public Func<Task> DismissTask { get; set; }

            public CircleView(Context context) : base(context)
            {
                SetClipChildren(false);
            }

            public class NoTouchView : View
            {
                private ShapeDrawable _backgroundShape;
                private int _radius;
                public NoTouchView(Context context) : base(context)
                {
                }

                public override bool OnTouchEvent(MotionEvent e)
                {
                    var xTouch = e.GetX();
                    var yTouch = e.GetY();

                    double centerX = _radius/2;
                    double centerY = _radius/2;
                    double distanceX = xTouch - centerX;
                    double distanceY = yTouch - centerY;
                    return (distanceX * distanceX) + (distanceY * distanceY) <= _radius * _radius / 4;
                }

                public void ToCircle(Context context, int radius, Color color, int alpha)
                {
                    _backgroundShape = new ShapeDrawable(new OvalShape());
                    var paint = new Paint
                    {
                        Color = color
                    };
                    paint.Alpha = alpha;
                    paint.SetStyle(Paint.Style.Fill);
                    _backgroundShape.Paint.Set(paint);
                    _backgroundShape.SetBounds(0, 0, radius.ToDP(context), radius.ToDP(context));
                    Background = _backgroundShape;
                    _radius = radius;
                }
            }

            public void BuildView(int radius, int alpha)
            {
                var view = new NoTouchView(Context)
                {
                    Id = GenerateViewId()
                };
                view.ToCircle(Context, radius, AppColorDefinition.MainBlue.ToAndroidColor(), alpha);
                var growAnimation = new ScaleAnimation(1, 1.02f, 1, 1.05f, Dimension.RelativeToSelf, 0.5f, Dimension.RelativeToSelf, 0.5f)
                {
                    Duration = 3000,
                    RepeatMode = RepeatMode.Reverse,
                    RepeatCount = ValueAnimator.Infinite,
                    StartOffset = radius
                };

                Animation = growAnimation;
                growAnimation.Start();
                var viewLayoutParameters = new LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)
                {
                    TopToTop = LayoutParams.ParentId,
                    BottomToBottom = LayoutParams.ParentId,
                    LeftToLeft = LayoutParams.ParentId,
                    RightToRight = LayoutParams.ParentId
                };
                AddView(view, viewLayoutParameters);
                var guideline = new Guideline(Context)
                {
                    LayoutParameters = new LayoutParams(0, 0)
                    {
                        Orientation = LayoutParams.Vertical,
                        GuidePercent = 0.5f,
                    },
                    Id = GenerateViewId()
                };
                AddView(guideline);
                if (_topItem != null)
                {
                    var topItemLayoutParameters = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
                    {
                        TopToTop = view.Id,
                        LeftToLeft = guideline.Id,
                    };
                    _topItem.SetBindings();

                    AddView(_topItem, topItemLayoutParameters);
                    _topItem.TranslationX = -15.ToDP(Context);
                    _topItem.TranslationY = -15.ToDP(Context);
                }
                if (_bottomItem != null)
                {
                    var bottomItemLayoutParameters = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
                    {
                        BottomToBottom = view.Id,
                        LeftToLeft = guideline.Id,
                    };
                    _bottomItem.SetBindings();
                    AddView(_bottomItem, bottomItemLayoutParameters);
                    _bottomItem.TranslationX = -15.ToDP(Context);
                    _bottomItem.TranslationY = 15.ToDP(Context);
                }
                if(WithCloseOption)
                {
                    DrawCross(DismissTask);
                }
            }

            private void DrawCross(Func<Task> dismissTask)
            {
                var crossLayoutParameters = new LayoutParams(10.ToDP(Context), 10.ToDP(Context))
                {
                    TopToTop = LayoutParams.ParentId,
                    LeftToLeft = LayoutParams.ParentId,
                    RightToRight = LayoutParams.ParentId,
                    BottomToBottom = LayoutParams.ParentId
                };

                var cross = new Cross(Context, AppColorDefinition.SecondaryColor.ToAndroidColor(), dismissTask);
                AddView(cross, crossLayoutParameters);
            }

            public class Cross : View
            {
                private readonly Color _color;
                private readonly Func<Task> _dismissTask;

                public Cross(Context context, Color color, Func<Task> dismissTask) : base(context)
                {
                    _color = color;
                    _dismissTask = dismissTask;
                    Click += Cross_Click;
                }

                async void Cross_Click(object sender, EventArgs e)
                {
                    Click -= Cross_Click;
                    await _dismissTask().ConfigureAwait(false);
                }

                protected override void OnDraw(Canvas canvas)
                {
                    float width = MeasuredWidth;
                    float height = MeasuredHeight;
                    Paint paint = new Paint
                    {
                        Color = _color
                    };
                    paint.SetStyle(Paint.Style.Stroke);
                    paint.StrokeWidth = 2.ToDP(Context);
                    canvas.DrawLine(0, 0, width, height, paint);
                    canvas.DrawLine(width, 0, 0, height, paint);
                }
            }

            public void AddItemOnTop(MenuItemView item)
            {
                _topItem = item;
            }

            public void AddItemOnBottom(MenuItemView item)
            {
                _bottomItem = item;
            }
        }
    }
}
