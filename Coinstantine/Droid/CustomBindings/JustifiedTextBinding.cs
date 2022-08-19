using Android.Webkit;
using MvvmCross.Binding.Bindings.Target;

namespace Coinstantine.Droid.CustomBindings
{
    public class JustifiedTextBinding : MvxConvertingTargetBinding<WebView, string>
    {
        public JustifiedTextBinding(WebView target) : base(target)
        {
        }

        protected override void SetValueImpl(WebView target, string value)
        {
            if (target == null)
            {
                return;
            }
            target.LoadData(string.Format(Html, value), "text/html; charset=utf-8", "utf-8");
        }

        private string Html => @"<html><head></head><body style='text-align:justify;color:gray;background-color:white;'>{0}</body></html>";
    }
}
