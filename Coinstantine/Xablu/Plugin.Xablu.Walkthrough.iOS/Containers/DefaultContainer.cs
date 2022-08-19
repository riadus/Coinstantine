using System;
using BWWalkthrough;
using Foundation;
using Plugin.Xablu.Walkthrough.Abstractions.Containers;
using Plugin.Xablu.Walkthrough.Abstractions.Controls;
using Splat;

namespace Plugin.Xablu.Walkthrough.Containers
{
    public abstract class DefaultContainer : BWWalkthroughViewController, IDefaultContainer
    {
        public DefaultContainer(IntPtr handle) : base(handle)
        { }

        public DefaultContainer(NSCoder coder) : base(coder)
        { }

        public DefaultContainer(string nibName, NSBundle bundle) : base(nibName, bundle)
        { }

        public virtual SplatColor BackgroundColor { get; set; } = SplatColor.White;
        public PageControl CirclePageControl { get; set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = BackgroundColor.ToNative();
        }
    }
}
