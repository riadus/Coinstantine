using System;
using Splat;

namespace Plugin.Xablu.Walkthrough.Abstractions.Controls
{
    public class ButtonControl : TextControl
    {
        public SplatColor BackgroundColor;
        public Action ClickAction;
    }
}