using Foundation;
using ObjCRuntime;
using UIKit;

namespace Coinstantine.iOS.Views
{
    public static class ViewFactory
    {
        public static TView Create<TView>() where TView : UIView
        {
            var name = typeof(TView).Name;
            var arr = NSBundle.MainBundle.LoadNib(name, null, null);
            return Runtime.GetNSObject<TView>(arr.ValueAt(0));
        }
    }
}
