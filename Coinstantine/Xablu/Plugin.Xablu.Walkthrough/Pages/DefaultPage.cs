using Plugin.Xablu.Walkthrough.Abstractions.Controls;
using Plugin.Xablu.Walkthrough.Abstractions.Pages;
using Splat;

namespace Plugin.Xablu.Walkthrough.Pages
{
    public class DefaultPage : IDefaultPage
    {
        public SplatColor BackgroundColor { get; set; }
        public TextControl TitleControl { get; set; }
        public ImageControl ImageControl { get; set; }
        public TextControl DescriptionControl { get; set; }
    }
}