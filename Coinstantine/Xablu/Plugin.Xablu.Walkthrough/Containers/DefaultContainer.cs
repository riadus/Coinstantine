using Plugin.Xablu.Walkthrough.Abstractions.Controls;
using Plugin.Xablu.Walkthrough.Abstractions.Containers;
using Splat;

namespace Plugin.Xablu.Walkthrough.Containers
{
    public abstract class DefaultContainer : IDefaultContainer
    {
        public virtual SplatColor BackgroundColor { get; set; }
        public virtual PageControl CirclePageControl { get; set; }
    }
}