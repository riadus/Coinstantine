using System;
using UIKit;

namespace Coinstantine.iOS.Views.Menu
{
    public class GestureDelegate : UIGestureRecognizerDelegate
    {
        private readonly Type _type;

        public GestureDelegate(Type type)
        {
            _type = type;
        }

        public override bool ShouldReceiveTouch(UIGestureRecognizer recognizer, UITouch touch)
        {
            return touch.View.Superview.GetType() != _type;
        }
    }
}
