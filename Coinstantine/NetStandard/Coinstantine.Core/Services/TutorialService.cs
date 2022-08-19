using System;
using System.Collections.Generic;
using Coinstantine.Common.Attributes;
using Coinstantine.Core.Extensions;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Translations;
using Plugin.Xablu.Walkthrough;
using Plugin.Xablu.Walkthrough.Abstractions.Controls;
using Plugin.Xablu.Walkthrough.Containers;
using Plugin.Xablu.Walkthrough.Pages;
using Plugin.Xablu.Walkthrough.Themes;

namespace Coinstantine.Core.Services
{
	[RegisterInterfaceAsDynamic]
	public class TutorialService : ITutorialService
	{
		private readonly ICrossWalkthroughInitializer _initializer;
		private readonly ITranslationService _translationService;

		public TutorialService(ICrossWalkthroughInitializer initializer,
							   ITranslationService translationService)
		{
			_initializer = initializer;
			_translationService = translationService;
		}

		private string Translate(TranslationKey translationKey)
		{
			return _translationService.Translate(translationKey);
		}

		public void StartTelegramTutorial(Action startAction)
		{
			_initializer.Inititalize();
			BuildPages();
			var theme = new Theme<ForestPrimesPage, ForestPrimesContainer>
			{
				Container = new ForestPrimesContainer
				{
					SkipButtonControl = new ButtonControl
					{
						Text = Translate(TranslationKeys.TelegramTutorial.Skip),
						TextColor = AppColorDefinition.TutorialToolbarText.ToSplatColor(),
						BackgroundColor = AppColorDefinition.TutorialToolbarBackground.ToSplatColor(),
						ClickAction = CrossWalkthrough.Current.Close,
					},
					StartButtonControl = new ButtonControl
					{
						Text = Translate(TranslationKeys.TelegramTutorial.Start),
						BackgroundColor = AppColorDefinition.TutorialToolbarBackground.ToSplatColor(),
						TextSize = 16,
						TextColor = AppColorDefinition.TutorialToolbarText.ToSplatColor(),
						ClickAction = () =>
						{
							CrossWalkthrough.Current.Close();
							if (startAction != null)
							{
								startAction();
							}
						}
					},
					NextButtonControl = new ImageButtonControl
					{
						Animation = AnimationType.Curve,
						Image = "ArrowRight",
						ClickAction = CrossWalkthrough.Current.Next,
						TintColor = AppColorDefinition.TutorialToolbarText.ToSplatColor(),
					},
					CirclePageControl = new PageControl
					{
						SelectedPageColor = AppColorDefinition.TutorialToolbarSelectedPage.ToSplatColor(),
						UnSelectedPageColor = AppColorDefinition.TutorialToolbarUnselectedPage.ToSplatColor()
					}
				},

				Pages = _forestPages
			};

			CrossWalkthrough.Current.Setup(theme);
			CrossWalkthrough.Current.Show();
		}

		private string Title1 => Translate(TranslationKeys.TelegramTutorial.Title1);
		private string Title2 => Translate(TranslationKeys.TelegramTutorial.Title2);
		private string Title3 => Translate(TranslationKeys.TelegramTutorial.Title3);
		private string Title4 => Translate(TranslationKeys.TelegramTutorial.Title4);

		private string Text1 => Translate(TranslationKeys.TelegramTutorial.Text1);
		private string Text2 => Translate(TranslationKeys.TelegramTutorial.Text2);
		private string Text3 => Translate(TranslationKeys.TelegramTutorial.Text3);
		private string Text4 => Translate(TranslationKeys.TelegramTutorial.Text4);

		private List<ForestPrimesPage> _forestPages;

		private void BuildPages()
		{
			_forestPages = new List<ForestPrimesPage>
    			{
    			new ForestPrimesPage
    			{
    				BackgroundColor = AppColorDefinition.TutorialBackground.ToSplatColor(),
    				TitleControl = new TextControl
    				{
    					Text = Title1,
    					TextSize = 24,
    					TextColor = AppColorDefinition.TutorialTitle.ToSplatColor()
    				},
    				ImageControl = new ImageControl
    				{
    					Image = "TelegramTutorial1"
    				},
    				DescriptionControl = new TextControl
    				{
    					Text = Text1,
    					TextSize = 16,
    					TextColor = AppColorDefinition.TutorialDescription.ToSplatColor()
    				}
    			},
    			new ForestPrimesPage
    			{
    				BackgroundColor = AppColorDefinition.TutorialBackground.ToSplatColor(),
    				TitleControl = new TextControl
    				{
    					Text = Title2,
    					TextSize = 28,
    					TextColor = AppColorDefinition.TutorialTitle.ToSplatColor()
    				},
    				ImageControl = new ImageControl
    				{
    					Image = "TelegramTutorial2"
    				},
    				DescriptionControl = new TextControl
    				{
    					Text = Text2,
    					TextSize = 18,
    					TextColor = AppColorDefinition.TutorialDescription.ToSplatColor()
    				}
    			},
    			new ForestPrimesPage
    			{
    				BackgroundColor = AppColorDefinition.TutorialBackground.ToSplatColor(),
    				TitleControl = new TextControl
    				{
    					Text = Title3,
    					TextSize = 24,
    					TextColor = AppColorDefinition.TutorialTitle.ToSplatColor()
    				},
    				ImageControl = new ImageControl
    				{
    					Image = "TelegramTutorial3"
    				},
    				DescriptionControl = new TextControl
    				{
    					Text = Text3,
    					TextSize = 16,
    					TextColor = AppColorDefinition.TutorialDescription.ToSplatColor()
    					}
    			},
    			new ForestPrimesPage()
				{
					BackgroundColor = AppColorDefinition.TutorialBackground.ToSplatColor(),
					TitleControl = new TextControl
					{
						Text = Title4,
						TextSize = 24,
						TextColor = AppColorDefinition.TutorialTitle.ToSplatColor()
					},
					ImageControl = new ImageControl
					{
						Image = "TelegramTutorial4"
					},
					DescriptionControl = new TextControl
					{
						Text = Text4,
						TextSize = 16,
						TextColor = AppColorDefinition.TutorialDescription.ToSplatColor()
					}
				}
			};
		}
	}
}
