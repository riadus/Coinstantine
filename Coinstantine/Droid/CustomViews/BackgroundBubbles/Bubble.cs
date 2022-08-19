using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Views;

namespace Coinstantine.Droid.CustomViews.BackgroundBubbles
{
    public class Bubble : View
    {
        private readonly BubbleConfig _config;

        public Bubble(Context context, BubbleConfig config) : base(context)
        {
            _config = config;
        }

        protected override void OnDraw(Canvas canvas)
        {
            var shape = new ShapeDrawable(new OvalShape());
            var paint = new Paint
            {
                Color = _config.BackgroundColor
            };
            paint.SetStyle(Paint.Style.Fill);
            shape.Paint.Set(paint);
            shape.SetBounds(0, 0, _config.Width, _config.Width);
            shape.Draw(canvas);

            var overlay = new ShapeDrawable(new OvalShape());
            var overlayPaint = new Paint
            {
                Color = Color.White,
                Alpha = _config.Alpha
            };
            overlayPaint.SetStyle(Paint.Style.Fill);
            overlay.Paint.Set(overlayPaint);

            overlay.SetBounds(0, 0, _config.Width, _config.Width);
            overlay.Draw(canvas);
        }
    }
}
