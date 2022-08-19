using System;
using CoreAnimation;
using Foundation;
using UIKit;

namespace Coinstantine.iOS.Views.Extensions
{
    public static class AnimationExtensions
    {
		public static void PulseToSize(this UIView view, float scale, double duration, bool repeat)
        {
            CABasicAnimation pulseAnimation = CABasicAnimation.FromKeyPath("transform.scale");
            pulseAnimation.Duration = duration;
            pulseAnimation.To = NSNumber.FromFloat(scale);
			pulseAnimation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);
            pulseAnimation.RepeatCount = repeat == false ? 0 : float.MaxValue;
			pulseAnimation.AutoReverses = true;
            view.Layer.AddAnimation(pulseAnimation, "pulse");
        }

        public static void PulseToSize(this UIView view, float scaleFrom, float scaleTo, double duration, bool repeat)
        {
            CABasicAnimation pulseAnimation = CABasicAnimation.FromKeyPath("transform.scale");
            pulseAnimation.Duration = duration;
            pulseAnimation.From = NSNumber.FromFloat(scaleFrom);
            pulseAnimation.To = NSNumber.FromFloat(scaleTo);
            pulseAnimation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);
            pulseAnimation.RepeatCount = repeat == false ? 0 : float.MaxValue;
            pulseAnimation.AutoReverses = repeat;
            view.Layer.AddAnimation(pulseAnimation, "pulse");
        }

		public static void PulseToSize(this UIView view, int pixels, double duration, bool repeat)
        {
			var scale = (float) ((view.Frame.Width + pixels) / view.Frame.Width);
            CABasicAnimation pulseAnimation = CABasicAnimation.FromKeyPath("transform.scale");
            pulseAnimation.Duration = duration;
            pulseAnimation.To = NSNumber.FromFloat(scale);
			pulseAnimation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);
            pulseAnimation.RepeatCount = repeat == false ? 0 : float.MaxValue;
            pulseAnimation.AutoReverses = true;
            view.Layer.AddAnimation(pulseAnimation, "pulse");
        }

		public static void ExpandToSize(this UIView view, double duration)
		{
			var pulseAnimation = CABasicAnimation.FromKeyPath("transform.scale");
			pulseAnimation.Duration = duration;
			pulseAnimation.From = NSNumber.FromFloat(0);
			pulseAnimation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);
			pulseAnimation.RepeatCount = 0;
                        
			view.Layer.AddAnimation(pulseAnimation, "expandToSize");
		}

		public static void ShrinkToSize(this UIView view, double duration)
		{
			var pulseAnimation = CABasicAnimation.FromKeyPath("transform.scale");
            pulseAnimation.Duration = duration;
            pulseAnimation.To = NSNumber.FromFloat(0.91f);
			pulseAnimation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);
            pulseAnimation.RepeatCount = 0;
			view.Layer.AddAnimation(pulseAnimation, "shrinkToSize");
		}

		public static void ShrinkToEmpty(this UIView view, double duration, Action onCompleted)
        {
			CATransaction.Begin();
            var pulseAnimation = CABasicAnimation.FromKeyPath("transform.scale");
            pulseAnimation.Duration = duration;
			pulseAnimation.To = NSNumber.FromFloat(0);
            pulseAnimation.From = NSNumber.FromFloat(1);
			pulseAnimation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);
            pulseAnimation.RepeatCount = 0;
			CATransaction.CompletionBlock = onCompleted;
			view.Layer.AddAnimation(pulseAnimation, "shrinkToEmpty");
			CATransaction.Commit();
        }

		public static void ShakeHorizontally(this UIView view, float strength = 12.0f)
        {
            CAKeyFrameAnimation animation = CAKeyFrameAnimation.FromKeyPath("transform.translation.x");
            animation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.Linear);
            animation.Duration = 0.5;
            animation.Values = new NSObject[]
                {
                    NSNumber.FromFloat(-strength),
                    NSNumber.FromFloat(strength),
                    NSNumber.FromFloat(-strength*0.66f),
                    NSNumber.FromFloat(strength*0.66f),
                    NSNumber.FromFloat(-strength*0.33f),
                    NSNumber.FromFloat(strength*0.33f),
                    NSNumber.FromFloat(0)
                };

            view.Layer.AddAnimation(animation, "shake");
        }
    }
}
