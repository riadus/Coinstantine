using Plugin.Xablu.Walkthrough.Abstractions.Controls;
using Splat;

namespace Plugin.Xablu.Walkthrough.Abstractions.Containers
{
    public interface IDefaultContainer : IContainer
    {
        SplatColor BackgroundColor { get; set; }
        PageControl CirclePageControl { get; set; }
    }
}