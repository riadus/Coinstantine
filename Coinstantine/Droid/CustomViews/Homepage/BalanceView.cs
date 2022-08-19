using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Support.Constraints;
using Android.Util;
using Android.Widget;
using Coinstantine.Core.ViewModels.Home;
using Coinstantine.Droid.CustomViews.BindableLayouts;
using Coinstantine.Droid.Extensions;
using MvvmCross.Binding.BindingContext;

namespace Coinstantine.Droid.CustomViews.Homepage
{
    public class BalanceView : BindableConstraintLayout
    {
        private TextView TokenSymbol { get; set; }
        private TextView TokenName { get; set; }
        private TextView TokenBalance { get; set; }
        private TextView AvailableLater { get; set; }

        public BalanceView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            this.DelayBind(SetBindings);
            Initialize();
        }

        private void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<BalanceView, TokenBalanceViewModel>();

            bindingSet.Bind(TokenSymbol)
                      .For("AppString")
                      .To(vm => vm.Symbole);
            bindingSet.Bind(TokenName)
                      .To(vm => vm.Name);
            bindingSet.Bind(TokenBalance)
                      .To(vm => vm.Balance);
            bindingSet.Bind(AvailableLater)
                      .To(vm => vm.AvailableLaterText);
            bindingSet.Bind(AvailableLater)
                      .For(v => v.Visibility)
                      .To(vm => vm.AvailableLater)
                      .WithVisibilityConversion();

            bindingSet.Apply();
        }

        private void Initialize()
        {
            Inflate(Context, Resource.Layout.balance_view, this);
            TokenSymbol = FindViewById<TextView>(Resource.Id.balanceSymbolTextView);
            TokenName = FindViewById<TextView>(Resource.Id.balanceTokenNameTextView);
            TokenBalance = FindViewById<TextView>(Resource.Id.balanceBalanceTextView);
            AvailableLater = FindViewById<TextView>(Resource.Id.balanceAvailableLaterTextView);
            var innerView = FindViewById<ConstraintLayout>(Resource.Id.balanceInnerView);
            var shape = new ShapeDrawable(new OvalShape());
            var paint = new Paint
            {
                Color = Color.White
            };
            paint.SetStyle(Paint.Style.Stroke);
            paint.StrokeWidth = 2.ToDP(Context);
            shape.Paint.Set(paint);
            innerView.Background = shape;
            AvailableLater.TranslationY = 5.ToDP(Context);
            AvailableLater.SetBackgroundResource(Resource.Drawable.textview_corners);
        }
    }
}
