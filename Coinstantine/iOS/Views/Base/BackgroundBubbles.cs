using System;
using System.Collections.Generic;
using CoreGraphics;
using Coinstantine.Core;
using Coinstantine.iOS.Views.Extensions;
using Coinstantine.Core.Extensions;
using Coinstantine.Common;
using UIKit;
using Coinstantine.Core.Fonts;

namespace Coinstantine.iOS.Views.Base
{
	public class BackgroundBubbles : UIView
	{
		public BackgroundBubbles(CGRect frame) : base(frame)
        {
        }

        public string TitleIcon { get; set; }
        public string Title { get; set; }

		public override void WillMoveToSuperview(UIView newsuper)
		{
			base.WillMoveToSuperview(newsuper);
			var backgroundColor = AppColorDefinition.MainBlue.ToUIColor();

			var configs = new List<Bubble.BubbleConfig>{
				new Bubble.BubbleConfig{
					Percentage = 90,
					Alpha = 0.05f,
					BackgroundColor = backgroundColor
				},
				new Bubble.BubbleConfig{
					Percentage = 73,
					Alpha = 0.12f,
					BackgroundColor = backgroundColor
				},
				new Bubble.BubbleConfig{
					Percentage = 46,
					Alpha = 0.06f,
					BackgroundColor = backgroundColor
				},
                new Bubble.BubbleConfig
                {
                    Percentage = 67,
                    BackgroundColor = backgroundColor
                }
			};

            if(TitleIcon.IsNotNull())
            {
                var (Code, Font) = TitleIcon.ToAppFont();
                var isNegative = Font == FontType.FontAwesomeBrandNegative;
                configs.Add(new Bubble.BubbleConfig
                {
                    Percentage = 60,
                    BackgroundColor = isNegative ? backgroundColor : UIColor.White,
                    TitleIcon = new Bubble.TextConfig
                    {
                        Text = Code,
                        Font = TitleIcon.ToUIFont(isNegative ? 40 : 23),
                        TextColor = isNegative ? UIColor.White : backgroundColor
                    },
                    Title = new Bubble.TextConfig
                    {
                        Text = Title,
                        Font = UIFont.BoldSystemFontOfSize(17),
                        TextColor = UIColor.White
                    }
                });
            }

			var builder = new BubbleBuilder(configs);
			builder.Build(this);
		}

        public CGPoint BubbleCenter { get; set; }

        public class BubbleBuilder
		{
			public BubbleBuilder(IEnumerable<Bubble.BubbleConfig> configs)
			{
				Configs = configs;
			}

			public IEnumerable<Bubble.BubbleConfig> Configs { get; }

            public void Build(BackgroundBubbles parent)
			{
				var previousWidth = parent.Frame.Width;
                foreach(var config in Configs)
				{
					config.Width = config.Width > 0 ? config.Width : previousWidth * config.Percentage / 100;
					previousWidth = config.Width;
					var bubble = new Bubble(config);
					parent.Add(bubble);
                    bubble.Center = parent.BubbleCenter;
				}
			}
		}

		public class Bubble : UIView
		{
			public BubbleConfig Config { get; }
			public Bubble(BubbleConfig config)
			{
				Config = config;
			}
			public override void WillMoveToSuperview(UIView newsuper)
			{
				base.WillMoveToSuperview(newsuper);
				var width = Config.Width > 0 ? Config.Width : newsuper.Frame.Width;
				Frame = new CGRect(0, 0, width, width);
				BackgroundColor = Config.BackgroundColor;
				var overlay = new UIView
                {
                    BackgroundColor = UIColor.White,
					Frame = new CGRect(0, 0, width, width),
					Alpha = Config.Alpha
                };
				Layer.CornerRadius = width / 2;
				overlay.Layer.CornerRadius = width / 2;
				Add(overlay);
                if (Config.TitleIcon != null)
                {
                    /*var label = new CircleAppStringLabel
                    {
                        Frame = new CGRect(0, 0, width, width)
                    };
                    label.TitleLabel.Text = Config.TitleIcon.Text;
                    label.TitleLabel.Font = Config.TitleIcon.Font;
                    label.IsNegative = Config.NegativeIcon;
                    Add(label);
                    */
                    var label = new UILabel
                    {
                        Text = Config.TitleIcon.Text,
                        Font = Config.TitleIcon.Font,
                        TextColor = Config.TitleIcon.TextColor
                    };
                    var size = label.SizeThatFits(overlay.Frame.Size);
                    label.Frame = new CGRect((overlay.Frame.Width / 2) - (size.Width / 2), (overlay.Frame.Height / 2) - (size.Height / 2), size.Width, size.Height);
                    Add(label);
                }
                if(Config.Title != null)
                {
                    var label = new UILabel
                    {
                        Text = Config.Title.Text,
                        Font = Config.Title.Font,
                        TextColor = Config.Title.TextColor
                    };
                    var size = label.SizeThatFits(overlay.Frame.Size);
                    label.Frame = new CGRect(overlay.Frame.Width * 1.5, (overlay.Frame.Height / 2) - (size.Height / 2), size.Width, size.Height);
                    Add(label);
                }
			}

			public class BubbleConfig
            {
                public nfloat Width { get; set; }
                public UIColor BackgroundColor { get; set; }
                public nfloat Alpha { get; set; }
				public nfloat Percentage { get; set; }
                public TextConfig TitleIcon { get; set; }
                public TextConfig Title { get; set; }
                public bool NegativeIcon { get; set; }
            }

            public class TextConfig
            {
                public string Text { get; set; }
                public UIFont Font { get; set; }
                public UIColor TextColor { get; set; }
            }
		}
	}
}

/*
 * using System;
using System.Collections.Generic;
using CoreGraphics;
using Coinstantine.Core;
using Coinstantine.iOS.Views.Extensions;
using Coinstantine.Core.Extensions;
using Coinstantine.Common;
using UIKit;
using Coinstantine.Core.Fonts;

namespace Coinstantine.iOS.Views.Base
{
    public class BackgroundBubbles : UIView
    {
        public BackgroundBubbles(CGRect frame) : base(frame)
        {
        }

        public string TitleIcon { get; set; }
        public string Title { get; set; }

        public override void WillMoveToSuperview(UIView newsuper)
        {
            base.WillMoveToSuperview(newsuper);
            var backgroundColor = AppColorDefinition.MainBlue.ToUIColor();

            var configs = new List<Bubble.BubbleConfig>{
                new Bubble.BubbleConfig{
                    Percentage = 90,
                    Alpha = 0.05f,
                    BackgroundColor = backgroundColor
                },
                new Bubble.BubbleConfig{
                    Percentage = 73,
                    Alpha = 0.12f,
                    BackgroundColor = backgroundColor
                },
                new Bubble.BubbleConfig{
                    Percentage = 46,
                    Alpha = 0.06f,
                    BackgroundColor = backgroundColor
                },
                new Bubble.BubbleConfig
                {
                    Percentage = 67,
                    BackgroundColor = backgroundColor
                }
            };

            if(TitleIcon.IsNotNull())
            {
                var (Code, Font) = TitleIcon.ToAppFont();
                var isNegative = Font == FontType.FontAwesomeBrandNegative;
                configs.Add(new Bubble.BubbleConfig
                {
                    Percentage = 67,
                    BackgroundColor = UIColor.White,
                    NegativeIcon = isNegative,
                    TitleIcon = new Bubble.TextConfig
                    {
                        Text = Code,
                        Font = TitleIcon.ToUIFont(23),
                        TextColor = backgroundColor
                    },
                    Title = new Bubble.TextConfig
                    {
                        Text = Title,
                        Font = UIFont.BoldSystemFontOfSize(17),
                        TextColor = UIColor.White
                    }
                });
            }

            var builder = new BubbleBuilder(configs);
            builder.Build(this);
        }

        public CGPoint BubbleCenter { get; set; }

        public class BubbleBuilder
        {
            public BubbleBuilder(IEnumerable<Bubble.BubbleConfig> configs)
            {
                Configs = configs;
            }

            public IEnumerable<Bubble.BubbleConfig> Configs { get; }

            public void Build(BackgroundBubbles parent)
            {
                var previousWidth = parent.Frame.Width;
                foreach(var config in Configs)
                {
                    config.Width = config.Width > 0 ? config.Width : previousWidth * config.Percentage / 100;
                    previousWidth = config.Width;
                    var bubble = new Bubble(config);
                    parent.Add(bubble);
                    bubble.Center = parent.BubbleCenter;
                }
            }
        }

        public class Bubble : UIView
        {
            public BubbleConfig Config { get; }
            public Bubble(BubbleConfig config)
            {
                Config = config;
            }
            public override void WillMoveToSuperview(UIView newsuper)
            {
                base.WillMoveToSuperview(newsuper);
                var width = Config.Width > 0 ? Config.Width : newsuper.Frame.Width;
                Frame = new CGRect(0, 0, width, width);
                BackgroundColor = Config.BackgroundColor;
                var overlay = new UIView
                {
                    BackgroundColor = UIColor.White,
                    Frame = new CGRect(0, 0, width, width),
                    Alpha = Config.Alpha
                };
                Layer.CornerRadius = width / 2;
                overlay.Layer.CornerRadius = width / 2;
                Add(overlay);
                if (Config.TitleIcon != null)
                {
                    var circleAppStringLabel = new CircleAppStringLabel();
                    circleAppStringLabel.TitleLabel.Text = Config.TitleIcon.Text;
                    circleAppStringLabel.TitleLabel.Font = Config.TitleIcon.Font;
                    circleAppStringLabel.TitleLabel.TextColor = Config.TitleIcon.TextColor;
                    if(Config.NegativeIcon)
                    {
                        circleAppStringLabel.IsNegative = true;
                    }
                    var size = circleAppStringLabel.SizeThatFits(overlay.Frame.Size);
                    circleAppStringLabel.Frame = overlay.Frame;
                    Add(circleAppStringLabel);
                }
                if(Config.Title != null)
                {
                    var label = new UILabel
                    {
                        Text = Config.Title.Text,
                        Font = Config.Title.Font,
                        TextColor = Config.Title.TextColor
                    };
                    var size = label.SizeThatFits(overlay.Frame.Size);
                    label.Frame = new CGRect(overlay.Frame.Width * 1.5, (overlay.Frame.Height / 2) - (size.Height / 2), size.Width, size.Height);
                    Add(label);
                }
            }

            public class BubbleConfig
            {
                public nfloat Width { get; set; }
                public UIColor BackgroundColor { get; set; }
                public nfloat Alpha { get; set; }
                public nfloat Percentage { get; set; }
                public TextConfig TitleIcon { get; set; }
                public bool NegativeIcon { get; set; }
                public TextConfig Title { get; set; }
            }

            public class TextConfig
            {
                public string Text { get; set; }
                public UIFont Font { get; set; }
                public UIColor TextColor { get; set; }
            }
        }
    }
}*/

