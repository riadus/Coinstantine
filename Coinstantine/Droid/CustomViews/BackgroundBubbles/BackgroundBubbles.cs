using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Support.Constraints;
using Android.Util;
using Android.Views;
using Android.Widget;
using Coinstantine.Common;
using Coinstantine.Core;
using Coinstantine.Core.Extensions;
using Coinstantine.Core.Fonts;
using Coinstantine.Droid.Extensions;

namespace Coinstantine.Droid.CustomViews.BackgroundBubbles
{
    public class BackgroundBubbles : ConstraintLayout
    {
        public BackgroundBubbles(Context context, int size, string titleIcon, string title) : base(context)
        {
            Size = size;
            TitleIcon = titleIcon;
            Title = title;
            BuildView();
        }

        public string TitleIcon { get; set; }
        public string Title { get; set; }
        public int Size { get; }

        public void SetTitle(string title)
        {
            _builder.SetTitle(title);
        }

        private void BuildView()
        {
            var backgroundColor = AppColorDefinition.MainBlue.ToAndroidColor();
            var centerX = Width / 4;
            var centerY = Height / 4;
            var center = new Point((int)centerX, (int)centerY);
            var configs = new List<BubbleConfig>{
                new BubbleConfig{
                    Percentage = 90,
                    Alpha = 13,
                    BackgroundColor = backgroundColor,
                    Center = center
                },
                new BubbleConfig{
                    Percentage = 73,
                    Alpha = 30,
                    BackgroundColor = backgroundColor,
                    Center = center
                },
                new BubbleConfig{
                    Percentage = 52,
                    Alpha = 15,
                    BackgroundColor = backgroundColor,
                    Center = center
                },
                new BubbleConfig
                {
                    Percentage = 67,
                    BackgroundColor = backgroundColor,
                    Center = center,
                    Title = new TextConfig
                    {
                        Text = Title,
                        Font = Typeface.DefaultBold,
                        TextSize = 17,
                        TextColor = Color.White
                    }
                }
            };

            if (TitleIcon.IsNotNull())
            {
                var (Code, Font) = TitleIcon.ToAppFont();
                var isNegative = Font == FontType.FontAwesomeBrandNegative;
                configs.Add(new BubbleConfig
                {
                    Percentage = 60,
                    BackgroundColor = isNegative ? backgroundColor : Color.White,
                    Center = center,
                    TitleIcon = new TextConfig
                    {
                        Text = Code,
                        Font = TitleIcon.ToTypeface(),
                        TextSize = isNegative ? 40 : 17,
                        TextColor = isNegative ? Color.White : backgroundColor,
                    }
                });
            }

            _builder = new BubbleBuilder(configs);
            _builder.BuildBubble(this);
        }

        private BubbleBuilder _builder;

        public class BubbleBuilder
        {
            private readonly List<BubbleConfig> _configs;
            private TextView _titleLabel;

            public void SetTitle(string title)
            {
                _titleLabel.Text = title;
            }

            public BubbleBuilder(List<BubbleConfig> configs)
            {
                _configs = configs;
            }

            public IEnumerable<View> BuildTextViews(BackgroundBubbles parent, IEnumerable<BubbleConfig> configs)
            {
                var previousWidth = parent.Width;
                foreach (var config in configs)
                {
                    config.Width = config.Width > 0 ? config.Width : previousWidth * config.Percentage / 100;
                    previousWidth = config.Width;
                    var bubble = new Bubble(parent.Context, config);
                    yield return bubble;
                }
            }

            public void BuildBubble(BackgroundBubbles parent)
            {
                var previousWidth = parent.Size;
                foreach (var config in _configs)
                {
                    config.Width = config.Width > 0 ? config.Width : previousWidth * config.Percentage / 100;
                    previousWidth = config.Width;
                    var bubble = new Bubble(parent.Context, config)
                    {
                        Id = GenerateViewId()
                    };
                    var layoutParams = new LayoutParams(config.Width, config.Width)
                    {
                        TopToTop = LayoutParams.ParentId,
                        BottomToBottom = LayoutParams.ParentId,
                        LeftToLeft = LayoutParams.ParentId,
                        RightToRight = LayoutParams.ParentId
                    };
                    parent.AddView(bubble, layoutParams);
                    if (config.TitleIcon != null)
                    {
                        var iconLabel = new TextView(parent.Context)
                        {
                            Text = config.TitleIcon.Text,
                            Typeface = config.TitleIcon.Font
                        };
                        iconLabel.SetTextSize(ComplexUnitType.Dip, config.TitleIcon.TextSize);
                        var iconLayoutParams = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
                        {
                            TopToTop = LayoutParams.ParentId,
                            BottomToBottom = LayoutParams.ParentId,
                            LeftToLeft = LayoutParams.ParentId,
                            RightToRight = LayoutParams.ParentId
                        };
                        iconLabel.SetTextColor(config.TitleIcon.TextColor);
                        parent.AddView(iconLabel, iconLayoutParams);
                    }
                    if (config.Title != null)
                    {
                        _titleLabel = new TextView(parent.Context)
                        {
                            Text = config.Title.Text,
                            Typeface = config.Title.Font
                        };
                        _titleLabel.Id = GenerateViewId();
                        _titleLabel.SetTextSize(ComplexUnitType.Dip, config.Title.TextSize);
                        var titleLayoutParams = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
                        {
                            TopToTop = LayoutParams.ParentId,
                            BottomToBottom = LayoutParams.ParentId,
                            LeftToRight = bubble.Id
                        };
                        _titleLabel.SetTextColor(config.Title.TextColor);
                        parent.AddView(_titleLabel, titleLayoutParams);
                    }
                }
            }
        }
    }
}
