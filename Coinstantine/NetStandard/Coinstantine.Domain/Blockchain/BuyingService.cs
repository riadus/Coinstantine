using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coinstantine.Common;
using Coinstantine.Common.Attributes;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Blockchain;

namespace Coinstantine.Domain.Blockchain
{
    [RegisterInterfaceAsDynamic]
    public class BuyingService : IBuyingService
    {
        private readonly IBackendService _backendService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProfileProvider _profileProvider;

        public BuyingService(IBackendService backendService,
                               IUnitOfWork unitOfWork,
                               IProfileProvider profileProvider)
        {
            _backendService = backendService;
            _unitOfWork = unitOfWork;
            _profileProvider = profileProvider;
        }

        public async Task<BuyingReceipt> Buy(decimal ethToInvest)
        {
            try
            {
                var profile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
                var ethAddress = profile.BlockchainInfo.Address;
                var purchase = await _backendService.BuyPresale(ethAddress, ethToInvest).ConfigureAwait(false);

                await _unitOfWork.BuyingReceipts.SaveAsync(purchase.ActualPurchase).ConfigureAwait(false);
                return purchase.ActualPurchase;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<BuyingReceipt>> GetReceipts()
        {
            var profile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
            return await _unitOfWork.BuyingReceipts.GetAllAsync(x => x.UserId == profile.Id);
        }

        public async Task<IEnumerable<BuyingReceipt>> SyncReceipts()    
        {
            var purchases = await _backendService.GetPurchases().ConfigureAwait(false);
            if (purchases?.Any() ?? false)
            {
                var profile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
                purchases = purchases.ForeachChangeValue(purchase => purchase.UserId = profile.Id);
                await _unitOfWork.BuyingReceipts.ReplaceAll(purchases).ConfigureAwait(false);
            }
            return purchases ?? new List<BuyingReceipt>();
        }
    }
}
