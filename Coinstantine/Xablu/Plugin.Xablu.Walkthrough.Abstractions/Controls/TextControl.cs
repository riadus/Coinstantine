using Splat;

namespace Plugin.Xablu.Walkthrough.Abstractions.Controls
{
    public class TextControl : BaseControl
    {
        public string Text { get; set; }
        public SplatColor TextColor { get; set; } = SplatColor.FromArgb(0, 0, 0);
        public float TextSize { get; set; } = 16.0f;
        public int TextStyle { get; set; } = 0;
    }
}