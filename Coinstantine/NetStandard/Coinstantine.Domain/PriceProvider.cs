using System.Linq;
using System.Threading.Tasks;
using Coinstantine.Common.Attributes;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Blockchain;

namespace Coinstantine.Domain
{
    [RegisterInterfaceAsDynamic]
    public class PriceProvider : IPriceProvider
    {
        private readonly IBackendService _backendService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlockchainService _blockchainService;
        private readonly IProfileProvider _profileProvider;

        public PriceProvider(IBackendService backendService,
                             IUnitOfWork unitOfWork,
                             IBlockchainService blockchainService,
                             IProfileProvider profileProvider)
        {
            _backendService = backendService;
            _unitOfWork = unitOfWork;
            _blockchainService = blockchainService;
            _profileProvider = profileProvider;
        }

        public async Task<CoinstantinePriceConfig> GetCoinstantinePriceConfig()
        {
            var configs = await _unitOfWork.CoinstantinePriceConfigs.GetAsync().ConfigureAwait(false);
            if (configs?.Any() ?? false)
            {
                return configs.First();
            }
            return null;
        }

        public async Task<bool> SyncCoinstantinePriceConfig()
        {
            var presaleConfig = await _blockchainService.GetPresaleConfig().ConfigureAwait(false);
            if(presaleConfig == null || presaleConfig.BaseRate == 0)
            {
                return false;
            }
            var price = 1 / presaleConfig.BaseRate;
            var bonus = (presaleConfig.Rate - presaleConfig.BaseRate) / presaleConfig.BaseRate;
            bonus *= 100;
            var config = new CoinstantinePriceConfig
            {
                Eth = price,
                Bonus = (int)bonus,
                MinimumEth = presaleConfig.Minimum,
                StartDate = presaleConfig.StartDate,
                EndDate = presaleConfig.EndDate
            };

            if (config != null)
            {
                await _unitOfWork.CoinstantinePriceConfigs.ReplaceAll(config).ConfigureAwait(false);
                return true;
            }
            return false;
        }

        public async Task<Price> GetEthPrice()
        {
            var prices = await _unitOfWork.Prices.GetAsync().ConfigureAwait(false);
            if (prices?.Any() ?? false)
            {
                return prices.First();
            }
            return null;
        }

        public async Task<bool> SyncEthPrice()
        {
            var price = await _backendService.GetEthPrice().ConfigureAwait(false);
            if (price != null)
            {
                await _unitOfWork.Prices.DeleteAllAsync().ConfigureAwait(false);
                await _unitOfWork.Prices.SaveAsync(price).ConfigureAwait(false);
                return true;
            }
            return false;
        }
    }
}