using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coinstantine.Core.Services;
using Coinstantine.Core.ViewModels.Generic;
using Coinstantine.Core.ViewModels.ProfileValidation;
using Coinstantine.Data;
using Coinstantine.Domain.Airdrops;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Airdrops;
using Coinstantine.Domain.Interfaces.Translations;
using MvvmCross.ViewModels;

namespace Coinstantine.Core.ViewModels.Aidrops
{
    public class AirdropViewModel : GenericInfoViewModel<AirdropDefinition>
    {
        private readonly IRequirementsConstructor _requirementsConstructor;
        private readonly IAirdropDefinitionsService _airdropDefinitionsService;
        private readonly INotificationsRegistrationService _notificationsRegistrationService;
        private AirdropDefinition _airdropDefinition;
        public AirdropViewModel(IAppServices appServices,
                                IProfileProvider profileProvider,
                                IUserService userService,
                                IGenericInfoItemViewModelConstructor itemInfoViewModelConstructor,
                                IRequirementsConstructor requirementsConstructor,
                                IAirdropDefinitionsService airdropDefinitionsService,
                                INotificationsRegistrationService notificationsRegistrationService) : base(appServices, profileProvider, userService, itemInfoViewModelConstructor)
        {
            Title = TranslationKeys.Home.Airdrop;
            TitleIcon = "airdrop";
            _requirementsConstructor = requirementsConstructor;
            _airdropDefinitionsService = airdropDefinitionsService;
            _notificationsRegistrationService = notificationsRegistrationService;
        }

        private string _infoTitle;

        public override string InfoTitle => _infoTitle;
        public override bool ShowRegularBehaviourText => false;
        protected override string AlternateBehaviourText => Translate(TranslationKeys.Airdrop.Subscribe);

        private string Subscribe => Translate(TranslationKeys.Airdrop.Subscribe);
        private string AlreadySubscribed => Translate(TranslationKeys.Airdrop.AlreadySubscribed);
        private string RequirementsNotMet => Translate(TranslationKeys.Airdrop.RequirementsNotMet);
        private string NotStartedYet => Translate(TranslationKeys.Airdrop.NotStartedYet);
        private string Expired => Translate(TranslationKeys.Airdrop.ExpiredDetail);
        private string Full => Translate(TranslationKeys.Airdrop.Full);
        private string InformationToBeShared => Translate(TranslationKeys.Airdrop.InformationToBeShared);
        private string UnknownAirdrop => Translate(TranslationKeys.Airdrop.UnknownAirdrop);

        protected override Task SecondaryButtonAction()
        {
            return Task.FromResult(0);
        }

        protected override int GetRemainingTime()
        {
            return 0;
        }

        protected override Task PrincipalButtonAction()
        {
            if (!AppServices.Connectivity.IsConnected)
            {
                ShowNoConnectionMessage();
                return Task.FromResult(0);
            }
            Alert(TranslationKeys.Airdrop.AlertBeforeSending, async () =>
            {
                DismissAlert(); 
                Wait(TranslationKeys.Airdrop.Subscribing);
                var (Success, FailReason) = await _airdropDefinitionsService.SubscribeToAirdrop(_airdropDefinition).ConfigureAwait(false);
                DismissWaitMessage();
                if (Success)
                {
                    if (await _notificationsRegistrationService.AreNotificationsEnabled().ConfigureAwait(false))
                    {
                        Alert(TranslationKeys.Airdrop.Success);
                    }
                    else
                    {
                        Alert(TranslationKeys.Airdrop.SuccessNoNotifications);
                    }
                }
                else
                {
                    Alert(TranslationKeys.Airdrop.Fail, GetArgument(FailReason));
                }
            }, Translate(TranslationKeys.Airdrop.Subscribe), Cancel);
            return Task.FromResult(0);
        }

        public override async void Prepare(AirdropDefinition parameter)
        {
            base.Prepare(parameter);
            TrackPage($"Aidrop page : {parameter.AirdropName}");
            _airdropDefinition = parameter;
            _infoTitle = _airdropDefinition.TokenName;

            var aidropDescription = new List<GenericInfoItemViewModel>(_itemInfoViewModelConstructor.Construct(
                new List<ItemInfo>
            {
                new ItemInfo
                {
                    Title = TranslationKeys.Airdrop.Description,
                    Display = Display.Title,
                    Value = parameter.OtherInfoToDisplay
                },
                new ItemInfo
                {
                    Title = TranslationKeys.Airdrop.AirdropName,
                    Display = Display.New,
                    Value = parameter.AirdropName
                },
                new ItemInfo
                {
                    Title = TranslationKeys.Airdrop.TokenName,
                    Display = Display.New,
                    Value = parameter.TokenName
                },
                new ItemInfo
                {
                    Title = TranslationKeys.Airdrop.Amount,
                    Display = Display.Grouped,
                    Value = parameter.Amount.ToString()
                },
                new ItemInfo
                {
                    Title = TranslationKeys.Airdrop.MaxParticipants,
                    Display = Display.New,
                    Value = parameter.MaxLimit.ToString()
                },
                new ItemInfo
                {
                    Title = TranslationKeys.Airdrop.NumberOfParticipants,
                    Display = Display.Grouped,
                    Value = parameter.NumberOfSubscribers.ToString()
                },
                new ItemInfo
                {
                    Title = TranslationKeys.Airdrop.StartDate,
                    Display = Display.New,
                    Value = FormatM(parameter.StartDate)
                },
                new ItemInfo
                {
                    Title = TranslationKeys.Airdrop.EndDate,
                    Display = Display.Grouped,
                    Value =FormatM(parameter.ExpirationDate)
                }
            }
            ));

            var twitterRequirements = _requirementsConstructor.BuildRequirements(parameter.TwitterAirdropRequirement, _userProfile.TwitterProfile);
            var bitcoinTalkRequirements = _requirementsConstructor.BuildRequirements(parameter.TelegramAirdropRequirement, _userProfile.TelegramProfile);
            var telegramRequirements = _requirementsConstructor.BuildRequirements(parameter.BitcoinTalkAirdropRequirement, _userProfile.BitcoinTalkProfile);

            var requirements = twitterRequirements
                                      .Union(bitcoinTalkRequirements)
                                         .Union(telegramRequirements);

            var requirementItemViewModels = new List<GenericInfoItemViewModel>(_itemInfoViewModelConstructor.Construct(requirements, typeof(bool)));

            var requirementsMet = _requirementsConstructor.MeetsAllRequirement(_userProfile, new List<IAirdropRequirement>
            {
                parameter.TwitterAirdropRequirement,
                parameter.TelegramAirdropRequirement,
                parameter.BitcoinTalkAirdropRequirement
            });

            var (StatusStr, SubscriptionIsPossible) = requirementsMet 
                                    ? GetValuesFromStatus(await _airdropDefinitionsService.GetStatus(_airdropDefinition).ConfigureAwait(false)) 
                                    : GetValuesFromStatus(AirdropStatus.RequirementNotMet);
            
            var requirementDescription = new List<GenericInfoItemViewModel>(_itemInfoViewModelConstructor.Construct(
                new List<ItemInfo>
            {
                new ItemInfo
                {
                    Title = TranslationKeys.Airdrop.Requirements,
                    Display = Display.Title,
                    Value = StatusStr
                }
            }));

            EnabledAction = SubscriptionIsPossible;
            GenericInfoItems = new MvxObservableCollection<GenericInfoItemViewModel>(aidropDescription
                                                                                     .Union(requirementDescription)
                                                                                     .Union(requirementItemViewModels));
        }

        private string GetArgument(FailReason failReason)
        {
            switch (failReason)
            {
                case FailReason.AlreadySubscribed:
                    return AlreadySubscribed;
                case FailReason.Expired:
                    return Expired;
                case FailReason.NotStarted:
                    return NotStartedYet;
                case FailReason.Full:
                    return Full;
                case FailReason.UnknownAirdrop:
                    return UnknownAirdrop;
                case FailReason.RequirementsNotMet:
                    return RequirementsNotMet;
                case FailReason.None:
                default:
                    return string.Empty;
            }
        }

        private (string StatusStr, bool SubscriptionIsPossible) GetValuesFromStatus(AirdropStatus status)
        {
            var subscriptionPossible = false;
            var result = string.Empty;
            switch(status)
            {
                case AirdropStatus.AlreadySubscribed:
                    result = AlreadySubscribed;
                    break;
                case AirdropStatus.Expired:
                    result = Expired;
                    break;
                case AirdropStatus.Full:
                    result = Full;
                    break;
                case AirdropStatus.NotStarted:
                    result = NotStartedYet;
                    break;
                case AirdropStatus.RequirementNotMet:
                    result = RequirementsNotMet;
                    break;
                case AirdropStatus.Ok:
                case AirdropStatus.SoonToBeFull:
                    subscriptionPossible = true;
                    result = InformationToBeShared;
                    break;
                default:
                    subscriptionPossible = false;
                    result = string.Empty;
                    break;
            }

            return (result, subscriptionPossible);
        }
    }
}