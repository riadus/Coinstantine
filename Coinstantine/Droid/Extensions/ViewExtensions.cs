using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Support.V4.Content;
using Android.Support.V4.Content.Res;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace Coinstantine.Droid.Extensions
{
    public static class ViewExtensions
    {
        public static void ShakeHorizontally(this View view, Context context)
        {
            var animation = AnimationUtils.LoadAnimation(context, Resource.Animation.shake);
            view.StartAnimation(animation);
        }

        public static int ToDP(this int pixels, Context context)
        {
            return (int)(context.Resources.DisplayMetrics.Density * pixels);
        }

        public static void SetDPBounds(this ShapeDrawable drawableShape, int x, int y, int width, int height, Context context)
        {
            drawableShape.SetBounds(x.ToDP(context), y.ToDP(context), width.ToDP(context), height.ToDP(context));
        }

        public static void ToCircle(this View view, Context context, int radius, Color color)
        {
            var shape = new ShapeDrawable(new OvalShape());
            var paint = new Paint
            {
                Color = color
            };
            paint.SetStyle(Paint.Style.Fill);
            shape.Paint.Set(paint);
            shape.SetBounds(0, 0, radius.ToDP(context), radius.ToDP(context));
            view.Background = shape;
        }

        public static void ToCircle(this View view)
        {
            var shape = new ShapeDrawable(new OvalShape());
            var paint = new Paint
            {
                Color = ((ColorDrawable)view.Background).Color
            };
            paint.SetStyle(Paint.Style.Fill);
            shape.Paint.Set(paint);
            shape.SetBounds(0, 0, view.Width, view.Height);
            view.Background = shape;
        }

        public static void SetCornerRadius(this View view, Context context, Color color)
        {
            var drawable = ResourcesCompat.GetDrawable(context.Resources, Resource.Drawable.whiteButton, null);
            drawable.SetColorFilter(color, PorterDuff.Mode.SrcIn);
            view.Background = drawable;
        }

        public static void SendViewToBack(this View view)
        {
            var parent = (ViewGroup)view.Parent;
            if (parent != null)
            {
                parent.RemoveView(view);
                parent.AddView(view, 0);
            }
        }

        public static void WithCornersAndShadow(this View view,
                                            Color backgroundColor,
                                            int cornerRadius,
                                            Color shadowColor,
                                            int elevation,
                                            int shadowGravity)
        {
            float[] outerRadius = {cornerRadius, cornerRadius, cornerRadius,
                cornerRadius, cornerRadius, cornerRadius, cornerRadius,
                cornerRadius};

            var backgroundPaint = new Paint();
            backgroundPaint.SetStyle(Paint.Style.Fill);
            backgroundPaint.SetShadowLayer(cornerRadius, 0, 0, backgroundColor);
            var backckgroundShape = new ShapeDrawable(new OvalShape());
            backckgroundShape.Paint.Set(backgroundPaint);

            var shapeDrawablePadding = new Rect
            {
                Left = elevation,
                Right = elevation
            };

            int DY;
            switch (shadowGravity)
            {
                case 0:
                    shapeDrawablePadding.Top = elevation;
                    shapeDrawablePadding.Bottom = elevation;
                    DY = 0;
                    break;
                case 1:
                    shapeDrawablePadding.Top = elevation * 2;
                    shapeDrawablePadding.Bottom = elevation;
                    DY = -1 * elevation / 3;
                    break;
                case 2:
                default:
                    shapeDrawablePadding.Top = elevation;
                    shapeDrawablePadding.Bottom = elevation * 2;
                    DY = elevation / 3;
                    break;
            }

            var shapeDrawable = new ShapeDrawable();
            shapeDrawable.SetPadding(shapeDrawablePadding);

            shapeDrawable.Paint.Color = backgroundColor;
            shapeDrawable.Paint.SetShadowLayer(elevation / 3, 0, DY, shadowColor);

            view.SetLayerType(LayerType.Software, shapeDrawable.Paint);

            shapeDrawable.Shape = new RoundRectShape(outerRadius, null, null);

            var drawable = new LayerDrawable(new Drawable[] { backckgroundShape, shapeDrawable });
            drawable.SetLayerInset(0, elevation, elevation * 2, elevation, elevation * 2);

            view.Background = drawable;
        }
    }
}