using System;
using System.Numerics;
using System.Threading.Tasks;
using Coinstantine.Common;
using Coinstantine.Common.Attributes;
using Coinstantine.Data;
using Coinstantine.Domain.Interface.Blockchain;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Blockchain;
using Nethereum.Util;

namespace Coinstantine.Domain.Blockchain
{
    [RegisterInterfaceAsDynamic]
    public class BlockchainService : IBlockchainService
    {
        private readonly ISmartContractProvider _smartContractProvider;
        private readonly IBackendService _backendService;
        private readonly IProfileProvider _profileProvider;
        private readonly IWeb3Provider _web3Provider;
        private readonly IUnitOfWork _unitOfWork;

        public BlockchainService(ISmartContractProvider smartContractProvider,
                                 IBackendService backendService,
                                 IProfileProvider profileProvider,
                                 IWeb3Provider web3Provider,
                                 IUnitOfWork unitOfWork)
        {
            _smartContractProvider = smartContractProvider;
            _backendService = backendService;
            _profileProvider = profileProvider;
            _web3Provider = web3Provider;
            _unitOfWork = unitOfWork;
        }

        private async Task<BigInteger> GetMOCoinstantineBalance(string address)
        {
            var smartContract = await _smartContractProvider.GetSmartContract("MOCoinstantine").ConfigureAwait(false);
            if (smartContract == null)
            {
                return 0;
            }
            var function = smartContract.GetFunction("balanceOf");
            try
            {
                return await function.CallAsync<BigInteger>(address);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private async Task<decimal> GetEthBalance(string address)
        {
            try
            {
                var web3 = await _web3Provider.GetWeb3().ConfigureAwait(false);
                var balance = await web3.Eth.GetBalance.SendRequestAsync(address);
                return Nethereum.Web3.Web3.Convert.FromWei(balance, UnitConversion.EthUnit.Ether);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<bool> UpdateBalance()
        {
            var profile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
            if(profile?.BlockchainInfo == null)
            {
                return false;
            }
            var moc = await GetMOCoinstantineBalance(profile.BlockchainInfo.Address).ConfigureAwait(false);
            var eth = await GetEthBalance(profile.BlockchainInfo.Address).ConfigureAwait(false);
            decimal mocDecimal = 0;
            try
            {
                mocDecimal = (decimal) moc;
            }
            catch (OverflowException)
            {
                return false;
            }

            profile.BlockchainInfo.Coinstantine = Math.Round(mocDecimal, 3);
            profile.BlockchainInfo.Ether = Math.Round(eth, 3);
            await _profileProvider.SaveUserProfile(profile).ConfigureAwait(false);
            return true;
        }

        public async Task<PresaleConfig> GetPresaleConfig()
        {
            try
            {
                var smartContract = await _smartContractProvider.GetSmartContract("Presale").ConfigureAwait(false);
                if (smartContract == null)
                {
                    return null;
                }
                var baseRateFunction = smartContract.GetFunction("baseRate");
                var baseRate = await baseRateFunction.CallAsync<BigInteger>();

                var rateFunction = smartContract.GetFunction("getRate");
                var rate = await rateFunction.CallAsync<BigInteger>();

                var minimumFunction = smartContract.GetFunction("minimumParticipationAmount");
                var minimum = await minimumFunction.CallAsync<BigInteger>();
                var minimumDecimal = UnitConversion.Convert.FromWei(minimum);

                var startDateFunction = smartContract.GetFunction("startDate");
                var startDateTimestamp = await startDateFunction.CallAsync<long>();
                var startDate = startDateTimestamp.ToDateTime();

                var endDateFunction = smartContract.GetFunction("endDate");
                var endDateTimestamp = await endDateFunction.CallAsync<long>();
                var endDate = endDateTimestamp.ToDateTime();

                return new PresaleConfig
                {
                    BaseRate = (decimal)baseRate,
                    Rate = (decimal)rate,
                    Minimum = minimumDecimal,
                    StartDate = startDate,
                    EndDate = endDate
                };
            }
            catch(Exception)
            {
                return null;
            }

        }

        public async Task<string> WithdrawBalance(string toAddress)
        {
            if(!await CanWithdrawBalance())
            {
                return string.Empty;
            }
            var profile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
            return await _backendService.WithdrawBalance(profile.BlockchainInfo.Address, toAddress);
        }

        public async Task<bool> CanWithdrawBalance()
        {
            var profile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
            return profile?.BitcoinTalkProfile?.Location != null;
        }
    }
}
