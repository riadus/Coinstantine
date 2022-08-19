using System;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;

namespace Coinstantine.Droid.CustomViews.Homepage
{
    public class AppRecyclerView : RecyclerView
    {
        private bool _scrolled;

        public AppRecyclerView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            SetLayoutManager(new AppLinearLayoutManager(Context, false));
        }

        public AppRecyclerView(Context context) : base(context)
        {
            SetLayoutManager(new AppLinearLayoutManager(Context, false));
        }

        public void EnableScroll()
        {
            AppLinearLayoutManager.SetCanScroll(true);
        }

        public void DisableScroll()
        {
            AppLinearLayoutManager.SetCanScroll(false);
        }

        AppLinearLayoutManager AppLinearLayoutManager => GetLayoutManager() as AppLinearLayoutManager;

        public override void OnScrollStateChanged(int state)
        {
            base.OnScrollStateChanged(state);
            switch (state)
            {
                case ScrollStateIdle:
                    if (_scrolled)
                    {
                        DisableScroll();
                        _scrolled = false;
                    }
                    break;
                case ScrollStateSettling:
                    _scrolled = true;
                    break;
            }
        }

        public event EventHandler Scrolled;

        public int CurrentPosition { get; protected set; }

        private void OnScrolled()
        {
            Scrolled?.Invoke(this, EventArgs.Empty);
        }

        public override void ScrollToPosition(int position)
        {
            CurrentPosition = position;
            OnScrolled();
            base.ScrollToPosition(position);
        }

        public override void SmoothScrollToPosition(int position)
        {
            CurrentPosition = position;
            OnScrolled();
            base.SmoothScrollToPosition(position);
        }
    }

    public class AppLinearLayoutManager : LinearLayoutManager
    {
        private readonly Context _context;
        private bool _canScroll;

        public AppLinearLayoutManager(Context context, bool canScroll) : base(context)
        {
            Orientation = Horizontal;
            _context = context;
            _canScroll = canScroll;
        }

        public void SetCanScroll(bool canScroll)
        {
            _canScroll = canScroll;
        }

        public override bool CanScrollVertically()
        {
            return Orientation == Vertical ? _canScroll : base.CanScrollVertically();
        }

        public override bool CanScrollHorizontally()
        {
            return Orientation == Horizontal ? _canScroll : base.CanScrollHorizontally();
        }

        public override void SmoothScrollToPosition(RecyclerView recyclerView, RecyclerView.State state, int position)
        {
            var linearScroller = new AppLinearSmoothScroller(_context)
            {
                TargetPosition = position
            };
            StartSmoothScroll(linearScroller);
        }
    }

    public class AppLinearSmoothScroller : LinearSmoothScroller
    {
        private const float MillisecondsPerInch = 0.5f;
        public AppLinearSmoothScroller(Context context) : base(context)
        {
        }

        protected AppLinearSmoothScroller(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected override int VerticalSnapPreference => LinearSmoothScroller.SnapToStart;

        protected override float CalculateSpeedPerPixel(DisplayMetrics displayMetrics)
        {
            return MillisecondsPerInch / displayMetrics.Density;
        }
    }
}