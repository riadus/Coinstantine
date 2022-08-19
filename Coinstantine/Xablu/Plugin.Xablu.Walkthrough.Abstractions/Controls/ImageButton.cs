using System;
using Splat;

namespace Plugin.Xablu.Walkthrough.Abstractions.Controls
{
    public class ImageButtonControl : BaseControl
    {
        public string Image { get; set; }
        public Action ClickAction { get; set; }
        public SplatColor TintColor { get; set; }
    }
}