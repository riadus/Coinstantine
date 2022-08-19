using System;
using Android.Animation;
using Android.Annotation;
using Android.Content;
using Android.Content.Res;
using Android.Database;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Coinstantine.Droid.CustomViews.Homepage;
using static Android.Support.V4.View.ViewPager;

namespace Coinstantine.Droid.CustomViews.IconViews
{
    public class CircleIndicator : LinearLayout
    {
        private const int DEFAULT_INDICATOR_WIDTH = 5;
        private AppRecyclerView _recyclterView;

        private int _lastPosition = -1;
        private int _indicatorMargin = -1;
        private int _indicatorWidth = -1;
        private int _indicatorHeight = -1;

        private int _animatorResId = Resource.Animator.scale;
        private int _animatorReverseResId = 0;

        public int _indicatorBackgroundResId = Resource.Drawable.white_radius;
        public int _indicatorUnselectedBackgroundResId = Resource.Drawable.white_radius;

        public Drawable SelectedDrawable { get; set; }
        public Drawable UnselectedDrawable { get; set; }

        private Animator _animatorOut;
        private Animator _animatorIn;
        private Animator _immediateAnimatorOut;
        private Animator _immediateAnimatorIn;

        public DataSetObserver InternalDataSetObserver { get; set; }

        public CircleIndicator(Context context) : base(context)
        {
            Init(context, null);
        }

        public CircleIndicator(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(context, attrs);
        }

        public CircleIndicator(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs,
            defStyleAttr)
        {
            Init(context, attrs);
        }

        [TargetApi(Value = 21)] //lollipop
        public CircleIndicator(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context,
            attrs, defStyleAttr, defStyleRes)
        {
            Init(context, attrs);
        }

        private void Init(Context context, IAttributeSet attrs)
        {
            InternalDataSetObserver = new CircleDataSetObserver(this);
            HandleTypedArray(context, attrs);
            CheckIndicatorConfig(context);
        }

        private void HandleTypedArray(Context context, IAttributeSet attrs)
        {
            if (attrs == null)
                return;

            TypedArray typedArray = context.ObtainStyledAttributes(attrs, Resource.Styleable.CircleIndicator);

            _indicatorWidth = typedArray.GetDimensionPixelSize(Resource.Styleable.CircleIndicator_ci_width, -1);
            _indicatorHeight = typedArray.GetDimensionPixelSize(Resource.Styleable.CircleIndicator_ci_height, -1);
            _indicatorMargin = typedArray.GetDimensionPixelSize(Resource.Styleable.CircleIndicator_ci_margin, -1);

            _animatorResId = typedArray.GetResourceId(Resource.Styleable.CircleIndicator_ci_animator,
                Resource.Animator.scale);
            _animatorReverseResId =
                typedArray.GetResourceId(Resource.Styleable.CircleIndicator_ci_animator_reverse, 0);
            _indicatorBackgroundResId = typedArray.GetResourceId(Resource.Styleable.CircleIndicator_ci_drawable,
                Resource.Drawable.white_radius);
            _indicatorUnselectedBackgroundResId =
                typedArray.GetResourceId(Resource.Styleable.CircleIndicator_ci_drawable_unselected,
                    _indicatorBackgroundResId);

            Android.Widget.Orientation orientation =
                (Android.Widget.Orientation) typedArray.GetInt(Resource.Styleable.CircleIndicator_ci_orientation, -1);
            Orientation = (orientation == Android.Widget.Orientation.Vertical
                ? Android.Widget.Orientation.Vertical
                : Android.Widget.Orientation.Horizontal);

            int gravity = typedArray.GetInt(Resource.Styleable.CircleIndicator_ci_gravity, -1);
            SetGravity(gravity >= 0 ? (GravityFlags) gravity : GravityFlags.Center);

            typedArray.Recycle();
        }

        public void ConfigureIndicator(int indicatorWidth, int indicatorHeight, int indicatorMargin)
        {
            ConfigureIndicator(indicatorWidth, indicatorHeight, indicatorMargin, Resource.Animator.scale, 0,
                Resource.Drawable.white_radius, Resource.Drawable.white_radius);
        }

        public void ConfigureIndicator(int indicatorWidth, int indicatorHeight, int indicatorMargin, int animatorId,
            int animatorReverseId, int indicatorBackgroundId, int indicatorUnselectedBackgroundId)
        {
            _indicatorWidth = indicatorWidth;
            _indicatorHeight = indicatorHeight;
            _indicatorMargin = indicatorMargin;

            _animatorResId = animatorId;
            _animatorReverseResId = animatorReverseId;
            _indicatorBackgroundResId = indicatorBackgroundId;
            _indicatorUnselectedBackgroundResId = indicatorUnselectedBackgroundId;

            CheckIndicatorConfig(Context);
        }

        private void CheckIndicatorConfig(Context context)
        {
            _indicatorWidth = (_indicatorWidth < 0) ? dip2px(DEFAULT_INDICATOR_WIDTH) : _indicatorWidth;
            _indicatorHeight = (_indicatorHeight < 0) ? dip2px(DEFAULT_INDICATOR_WIDTH) : _indicatorHeight;
            _indicatorMargin = (_indicatorMargin < 0) ? dip2px(DEFAULT_INDICATOR_WIDTH) : _indicatorMargin;
            _animatorResId = (_animatorResId == 0) ? Resource.Animator.scale : _animatorResId;

            _animatorOut = CreateAnimatorOut(context);
            _immediateAnimatorOut = CreateAnimatorOut(context);
            _immediateAnimatorOut.SetDuration(0);

            _animatorIn = CreateAnimatorIn(context);
            _immediateAnimatorIn = CreateAnimatorIn(context);
            _immediateAnimatorIn.SetDuration(0);

            _indicatorBackgroundResId = (_indicatorBackgroundResId == 0)
                ? Resource.Drawable.white_radius
                : _indicatorBackgroundResId;
            _indicatorUnselectedBackgroundResId = (_indicatorUnselectedBackgroundResId == 0)
                ? _indicatorBackgroundResId
                : _indicatorUnselectedBackgroundResId;
        }

        private Animator CreateAnimatorOut(Context context)
        {
            return AnimatorInflater.LoadAnimator(context, _animatorResId);
        }

        private Animator CreateAnimatorIn(Context context)
        {
            Animator animatorIn;
            if (_animatorReverseResId == 0)
            {
                animatorIn = AnimatorInflater.LoadAnimator(context, _animatorResId);
                animatorIn.SetInterpolator(new ReverseInterpolator());
            }
            else
            {
                animatorIn = AnimatorInflater.LoadAnimator(context, _animatorReverseResId);
            }
            return animatorIn;
        }

        public void SetRecyclerView(AppRecyclerView recyclerView)
        {
            _recyclterView = recyclerView;
            if (_recyclterView != null && _recyclterView.GetAdapter()  != null)
            {
                _lastPosition = -1;
                CreateIndicators();
                _recyclterView.Scrolled -= RecyclterView_Scrolled;
                _recyclterView.Scrolled += RecyclterView_Scrolled;
                OnPageSelected(_recyclterView.CurrentPosition);
            }
        }

        void RecyclterView_Scrolled(object sender, EventArgs e)
        {
            OnPageSelected(_recyclterView.CurrentPosition);
        }


        protected override void OnDetachedFromWindow()
        {
            _recyclterView.Scrolled -= RecyclterView_Scrolled;
            base.OnDetachedFromWindow();
        }

        public void OnPageSelected(int position)
        {
            if (_recyclterView.GetAdapter()?.ItemCount <= 0)
                return;

            if (_animatorIn.IsRunning)
            {
                _animatorIn.End();
                _animatorIn.Cancel();
            }

            if (_animatorOut.IsRunning)
            {
                _animatorOut.End();
                _animatorOut.Cancel();
            }

            View currentIndicator;
            if (_lastPosition >= 0 && (currentIndicator = GetChildAt(_lastPosition)) != null)
            {
                if (SelectedDrawable != null)
                    currentIndicator.Background = SelectedDrawable;
                else
                    currentIndicator.SetBackgroundResource(_indicatorUnselectedBackgroundResId);

                _animatorIn.SetTarget(currentIndicator);
                _animatorIn.Start();
            }

            View selectedIndicator = GetChildAt(position);
            if (selectedIndicator != null)
            {
                if (SelectedDrawable != null)
                    selectedIndicator.Background = SelectedDrawable;
                else
                    selectedIndicator.SetBackgroundResource(_indicatorBackgroundResId);

                _animatorOut.SetTarget(selectedIndicator);
                _animatorOut.Start();
            }
            _lastPosition = position;
        }

        public void SetColors(Color selectedColor, Color unselectedColor)
        {
            SelectedDrawable = CreateRoundedIndicator(selectedColor);
            UnselectedDrawable = CreateRoundedIndicator(unselectedColor);
        }

        private static ShapeDrawable CreateRoundedIndicator(Color color)
        {
            var drawable = new ShapeDrawable(new OvalShape());
            drawable.Paint.Color = color;
            return drawable;
        }

        private void CreateIndicators()
        {
            RemoveAllViews();

            int count = _recyclterView.GetAdapter()?.ItemCount ?? 0;
            if (count <= 0)
                return;

            int currentItem = _recyclterView.CurrentPosition;

            for (int i = 0; i < count; i++)
            {
                if (currentItem == i)
                {
                    if (SelectedDrawable != null)
                        AddIndicator(Orientation, SelectedDrawable, _immediateAnimatorOut);
                    else
                        AddIndicator(Orientation, _indicatorBackgroundResId, _immediateAnimatorOut);
                }
                else
                {
                    if (UnselectedDrawable != null)
                        AddIndicator(Orientation, UnselectedDrawable, _immediateAnimatorIn);
                    else
                        AddIndicator(Orientation, _indicatorBackgroundResId, _immediateAnimatorIn);
                }
            }
        }

        private void AddIndicator(Android.Widget.Orientation orientation, Drawable backgroundDrawableId,
            Animator animator)
        {
            if (animator.IsRunning)
            {
                animator.End();
                animator.Cancel();
            }

            var indicator = new View(Context)
            {
                Background = backgroundDrawableId
            };
            AddView(indicator, _indicatorWidth, _indicatorHeight);
            LayoutParams lp = (LayoutParams) indicator.LayoutParameters;

            if (orientation == Android.Widget.Orientation.Horizontal)
            {
                lp.LeftMargin = _indicatorMargin;
                lp.RightMargin = _indicatorMargin;
            }
            else
            {
                lp.TopMargin = _indicatorMargin;
                lp.BottomMargin = _indicatorMargin;
            }

            indicator.LayoutParameters = lp;

            animator.SetTarget(indicator);
            animator.Start();
        }

        private void AddIndicator(Android.Widget.Orientation orientation, int backgroundDrawableId, Animator animator)
        {
            if (animator.IsRunning)
            {
                animator.End();
                animator.Cancel();
            }

            var indicator = new View(Context);
            indicator.SetBackgroundResource(backgroundDrawableId);
            AddView(indicator, _indicatorWidth, _indicatorHeight);
            LayoutParams lp = (LayoutParams) indicator.LayoutParameters;

            if (orientation == Android.Widget.Orientation.Horizontal)
            {
                lp.LeftMargin = _indicatorMargin;
                lp.RightMargin = _indicatorMargin;
            }
            else
            {
                lp.TopMargin = _indicatorMargin;
                lp.BottomMargin = _indicatorMargin;
            }

            indicator.LayoutParameters = lp;

            animator.SetTarget(indicator);
            animator.Start();
        }

        class ReverseInterpolator : Java.Lang.Object, IInterpolator
        {
            public float GetInterpolation(float value)
            {
                return Math.Abs(1.0f - value);
            }
        }

        public int dip2px(float dpValue)
        {
            var density = Context.Resources.DisplayMetrics.Density;
            return (int) (dpValue * density + 0.5f);
        }

        class CircleDataSetObserver : DataSetObserver
        {
            private readonly CircleIndicator _indicator;

            public CircleDataSetObserver(CircleIndicator indicator)
            {
                _indicator = indicator;
            }

            public override void OnChanged()
            {
                base.OnChanged();
                if (_indicator._recyclterView == null)
                    return;

                int newCount = _indicator._recyclterView.GetAdapter().ItemCount;
                int currentCount = _indicator.ChildCount;

                if (newCount == currentCount)
                    return;
                else if (_indicator._lastPosition < newCount)
                    _indicator._lastPosition = _indicator._recyclterView.CurrentPosition;
                else
                    _indicator._lastPosition = -1;

                _indicator.CreateIndicators();
            }
        }
    }
}
