using System;
using Android.Content;
using Android.Graphics;
using Android.Support.Constraints;
using Android.Util;
using Android.Views;
using Android.Widget;
using Coinstantine.Droid.Extensions;

namespace Coinstantine.Droid.CustomViews.IconViews
{
    public class IconTextView : ConstraintLayout
    {
        public TextView Icon { get; private set; }
        public View Container { get; private set; }

        public IconTextView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize();
        }

        private void Initialize()
        {
            Inflate(Context, Resource.Layout.icon_text_view, this);
            Icon = FindViewById<TextView>(Resource.Id.iconTextViewIcon);
            Container = FindViewById<View>(Resource.Id.iconTextViewContainer);
            Container.ToCircle(Context, 30.ToDP(Context), Color.White);
        }

        internal void BigIcon()
        {
            Icon.SetTextSize(ComplexUnitType.Dip, 30);
        }

        internal void CircleIcon()
        {
            Container.ToCircle(Context, 30.ToDP(Context), Color.White);
        }
    }
}
