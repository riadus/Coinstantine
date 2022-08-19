using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Coinstantine.Common.Attributes;
using Coinstantine.Data;
using Coinstantine.Domain.Documents;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Airdrops;
using Coinstantine.Domain.Interfaces.Blockchain;
using Coinstantine.Domain.Messages;
using MvvmCross.Plugin.Messenger;
using Plugin.Connectivity.Abstractions;

namespace Coinstantine.Domain
{
	[RegisterInterfaceAsLazySingleton]
    public class SyncService : ISyncService
    {
		private readonly IBackendService _backendService;
        private readonly IUserService _userService;
        private readonly IConnectivity _connectivity;
		private readonly ITranslationService _translationService;
		private readonly IProfileProvider _profileProvider;
        private readonly IPriceProvider _priceProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlockchainService _blockchainService;
        private readonly IEnvironmentInfoProvider _environmentInfoProvider;
        private readonly IAirdropDefinitionsService _airdropDefinitionsService;
        private readonly IDocumentProvider _documentProvider;
        private readonly IMvxMessenger _mvxMessenger;
        private readonly IAnalyticsTracker _analyticsTracker;
        private const int SyncIntervalInHours = 24;

        public SyncService(IBackendService backendService,
                           IUserService userService,
                           IConnectivity connectivity,
                           ITranslationService translationService,
                           IProfileProvider profileProvider,
                           IPriceProvider priceProvider,
                           IUnitOfWork unitOfWork,
                           IBlockchainService blockchainService,
                           IEnvironmentInfoProvider environmentInfoProvider,
                           IAirdropDefinitionsService airdropDefinitionsService,
                           IDocumentProvider documentProvider,
                           IMvxMessenger mvxMessenger,
                           IAnalyticsTracker analyticsTracker)
        {
            _backendService = backendService;
            _userService = userService;
            _connectivity = connectivity;
            _translationService = translationService;
            _profileProvider = profileProvider;
            _priceProvider = priceProvider;
            _unitOfWork = unitOfWork;
            _blockchainService = blockchainService;
            _environmentInfoProvider = environmentInfoProvider;
            _airdropDefinitionsService = airdropDefinitionsService;
            _documentProvider = documentProvider;
            _mvxMessenger = mvxMessenger;
            _analyticsTracker = analyticsTracker;
        }

        public async Task<bool> SyncTranslations()
        {
            try
            {
                if (!_connectivity.IsConnected)
                {
                    return false;
                }
                var s = new Stopwatch();
                s.Start();
                var translations = await _backendService.GetTranslations().ConfigureAwait(false);
                s.Stop();
                Debug.WriteLine($"SyncService - Got Translations in {s.ElapsedMilliseconds} ms");
                s.Reset();
                s.Start();
                await _translationService.SaveTranslations(translations).ConfigureAwait(false);
                s.Stop();
                Debug.WriteLine($"SyncService - Saved Translations in {s.ElapsedMilliseconds} ms");
                _analyticsTracker.TrackSync(SyncCategory.Translations, s.ElapsedMilliseconds);
                return true;
            }
            catch
            {

            }
            return false;
        }

        private static bool UserNeedsToSync(UserProfile userProfile)
        {
            return userProfile?.LastSyncSession == null || userProfile.LastSyncSession.HasValue && DateTime.Now.Subtract(userProfile.LastSyncSession.Value).TotalHours >= SyncIntervalInHours;
        }

        public async Task SyncIfNeeded()
        {
            var userProfile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
            await CheckOnlineProfileAndUnpdateIfNeeded().ConfigureAwait(false);
            if (UserNeedsToSync(userProfile))
            {
                await DoSync(userProfile).ConfigureAwait(false);
            }
        }

        public async Task<bool> NeedsToSync()
        {
            var userProfile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
            return UserNeedsToSync(userProfile);
        }

        public async Task ForceSync()
        {
            var userProfile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
            await DoSync(userProfile).ConfigureAwait(false);
        }
        private bool IsSyncing;
        private async Task DoSync(UserProfile userProfile)
        {
            try
            {
                if (IsSyncing) return;
                IsSyncing = true;
                var s = new Stopwatch();
                s.Start();
                var result = await Task.WhenAll(new Task<bool>[]
                                {
                    SyncTranslations(),
                    SyncPrices(),
                    SyncSmartContractInfo(),
                    SyncEnvironmentInfo(),
                    SyncAirdropDefinitions(),
                    SyncDocuments(),
                    CheckOnlineProfileAndUnpdateIfNeeded()
                                }).ConfigureAwait(false);

                if (result.All(x => x) && userProfile != null)
                {
                    userProfile.LastSyncSession = DateTime.Now;
                    await _profileProvider.SaveUserProfile(userProfile).ConfigureAwait(false);
                }
                _mvxMessenger.Publish(new SyncDoneMessage(this));
                s.Stop();
                Debug.WriteLine($"SyncService - Whole Syncing took {s.ElapsedMilliseconds} ms");
                _analyticsTracker.TrackSync(SyncCategory.All, s.ElapsedMilliseconds);
            }
            finally
            {
                IsSyncing = false;
            }

        }

        public async Task<bool> SyncPrices()
        {
            if (!_connectivity.IsConnected)
            {
                return false;
            }
            var s = new Stopwatch();
            s.Start();
            var configSynced = await _priceProvider.SyncCoinstantinePriceConfig().ConfigureAwait(false);
            var result = configSynced && await _priceProvider.SyncEthPrice().ConfigureAwait(false);

            s.Stop();
            Debug.WriteLine($"SyncService - Synced SyncPrices in {s.ElapsedMilliseconds} ms");
            _analyticsTracker.TrackSync(SyncCategory.Prices, s.ElapsedMilliseconds);

            return result;
        }

        public async Task<bool> SyncSmartContractInfo()
        {
            if (!_connectivity.IsConnected)
            {
                return false;
            }
            var s = new Stopwatch();
            s.Start();
            var presaleSmartContract = await _backendService.GetSmartContractDefinition("Presale").ConfigureAwait(false);
            var moCoinstantineSmartContract = await _backendService.GetSmartContractDefinition("MOCoinstantine").ConfigureAwait(false);
            if (presaleSmartContract != null && moCoinstantineSmartContract != null)
            {
                await _unitOfWork.SmartContractDefinitions.ReplaceAll(new List<SmartContractDefinition> { presaleSmartContract, moCoinstantineSmartContract }).ConfigureAwait(false);
            }

            var result = await _blockchainService.UpdateBalance().ConfigureAwait(false);
            s.Stop();
            Debug.WriteLine($"SyncService - Synced SyncSmartContractInfo in {s.ElapsedMilliseconds} ms");
            _analyticsTracker.TrackSync(SyncCategory.SmartContractInfo, s.ElapsedMilliseconds);
            return result;
        }

        private async Task<bool> SyncEnvironmentInfo()
        {
            if (!_connectivity.IsConnected)
            {
                return false;
            }
            var s = new Stopwatch();
            s.Start();
            await _environmentInfoProvider.SetUrlsFromBackend().ConfigureAwait(false);
            s.Stop();
            Debug.WriteLine($"SyncService - Synced SyncEnvironmentInfo in {s.ElapsedMilliseconds} ms");
            _analyticsTracker.TrackSync(SyncCategory.EnvironmentInfo, s.ElapsedMilliseconds);

            return true;
        }

        public async Task<bool> SyncAirdropDefinitions()
        {
            var s = new Stopwatch();
            s.Start();
            var result = await _airdropDefinitionsService.SyncAirdropDefinitions().ConfigureAwait(false);
            s.Stop();
            Debug.WriteLine($"SyncService - Synced Airdrop Definitions in {s.ElapsedMilliseconds} ms");
            _analyticsTracker.TrackSync(SyncCategory.AirdropDefinitions, s.ElapsedMilliseconds);
            return result;
        }

        public async Task<bool> CheckOnlineProfileAndUnpdateIfNeeded()
        {
            var s = new Stopwatch();
            s.Start();
            var apiUser = await _backendService.GetOnlineUserProfile().ConfigureAwait(false);
            if(apiUser == null)
            {
                return false;
            }
            s.Stop();
            Debug.WriteLine($"SyncService - Got User Profile in {s.ElapsedMilliseconds} ms");
            s.Reset();
            s.Start();
            var userProfile = await _userService.UpdateProfile(apiUser).ConfigureAwait(false);
            s.Stop();
            Debug.WriteLine($"SyncService - Synced Profile in {s.ElapsedMilliseconds} ms");
            _analyticsTracker.TrackSync(SyncCategory.UserProfile, s.ElapsedMilliseconds);
            return true;
        }

        private async Task<bool> SyncDocuments()
        {
            var s = new Stopwatch();
            s.Start();
            var result = await _documentProvider.DownloadDocuments().ConfigureAwait(false);
            s.Stop();
            Debug.WriteLine($"SyncService - Synced Documents in {s.ElapsedMilliseconds} ms");
            _analyticsTracker.TrackSync(SyncCategory.Documents, s.ElapsedMilliseconds);

            return result;
        }
    }
}
