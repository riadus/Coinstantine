using System;
using System.Threading.Tasks;

namespace Coinstantine.Domain.Interfaces.Blockchain
{
    public interface IBlockchainService
    {
        Task<bool> UpdateBalance();
        Task<PresaleConfig> GetPresaleConfig();
        Task<string> WithdrawBalance(string toAddress);
        Task<bool> CanWithdrawBalance();
    }

    public class PresaleConfig
    {
        public decimal BaseRate { get; set; } 
        public decimal Rate { get; set; } 
        public decimal Minimum { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
