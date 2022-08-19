using Plugin.Xablu.Walkthrough.Abstractions.Controls;
using Splat;

namespace Plugin.Xablu.Walkthrough.Abstractions.Pages
{
    public interface IDefaultPage : IPage
    {
        SplatColor BackgroundColor { get; set; }
        TextControl TitleControl { get; set; }
        ImageControl ImageControl { get; set; }
        TextControl DescriptionControl { get; set; }
    }
}