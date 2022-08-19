using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Binding.Views;

namespace Coinstantine.Droid.CustomViews.Homepage
{
    public class AppScrollView : ScrollView
    {
        public AppScrollView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            Parent.RequestDisallowInterceptTouchEvent(true);
            return false;
        }
    }

    public class AppListView : MvxListView
    {
        public AppListView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public AppListView(Context context, IAttributeSet attrs, IMvxAdapter adapter) : base(context, attrs, adapter)
        {
        }

        protected AppListView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }
    }
}
