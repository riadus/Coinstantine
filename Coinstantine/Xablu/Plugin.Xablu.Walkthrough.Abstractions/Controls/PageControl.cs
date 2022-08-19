using Splat;
using System.Drawing;

namespace Plugin.Xablu.Walkthrough.Abstractions.Controls
{
    public class PageControl : BaseControl
    {
        public SplatColor SelectedPageColor { get; set; }
        public SplatColor UnSelectedPageColor { get; set; }
    }
}