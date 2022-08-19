using Android.Graphics;

namespace Coinstantine.Droid.CustomViews.BackgroundBubbles
{
    public class BubbleConfig
    {
        public int Width { get; set; }
        public Point Center { get; set; }
        public Color BackgroundColor { get; set; }
        public int Alpha { get; set; }
        public int Percentage { get; set; }
        public TextConfig TitleIcon { get; set; }
        public TextConfig Title { get; set; }
        public bool NegativeIcon { get; set; }
    }
}
