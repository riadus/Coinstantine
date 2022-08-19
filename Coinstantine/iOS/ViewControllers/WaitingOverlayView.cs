using System;
using System.Threading.Tasks;
using Airbnb.Lottie;
using MvvmCross.Platforms.Ios.Binding.Views;

namespace Coinstantine.iOS
{
    public partial class WaitingOverlayView : MvxView
    {
        public WaitingOverlayView (IntPtr handle) : base (handle)
        {
        }

        public void UpdateMessage(string message)
        {
            WaitingMessageLabel.Text = message;
        }

        public void SetActivityIndicator(bool tapToDismiss)
        {
            AnimationContainer.Hidden = tapToDismiss;
        }

        public async Task StartAnimation(int i)
        {
            if (i > 1) { i = 1; }
            if (i < 0) { i = 0; }
            var animation = LOTAnimationView.AnimationNamed($"Animations/Wait_{++i}");
            AnimationContainer.AddSubview(animation);
            var size = AnimationContainer.Frame.Width;
            nfloat x = 0, y = 0;
            if(i == 2)
            {
                x -= size / 2;
                y -= size;
                size *= 2;
            }
            else
            {
                y -= size / 4;
                x += size / 4;
                size /= 2;
            }
            animation.Frame = new CoreGraphics.CGRect(x, y, size, size);
            animation.LoopAnimation = true;
            //You can also use the awaitable version
            var animationFinished = await animation.PlayAsync();
        }
    }
}