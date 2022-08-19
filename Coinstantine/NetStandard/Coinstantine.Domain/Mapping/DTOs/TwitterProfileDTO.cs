using System;
using System.Collections.Generic;
using Coinstantine.Data;

namespace Coinstantine.Domain.Mapping.DTOs
{
    public class TwitterProfileDTO
    {
        public long TwitterId { get; set; }
        public string ScreenName { get; set; }
        public long TweetId { get; set; }
        public string Username { get; set; }
        public int NumberOfFollower { get; set; }
        public DateTime CreationDate { get; set; }
    }

    public class PriceDTO
    {
        public decimal Usd { get; set; }
        public decimal Eur { get; set; }
    }

    public class CoinstantinePriceConfigDTO 
    {
        public decimal Eth { get; set; }
        public int Bonus { get; set; }
        public decimal MinimumEth { get; set; }
        public decimal MaximumCsn { get; set; }
    }

    public class SmartContractDefinitionDTO
    {
        public string Abi { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
    }

    public class EnvironmentDTO
    {
        public string Environment { get; set; }
        public string Web3Url { get; set; }
        public string EtherscanUrl { get; set; }
        public string EthereumEnvironment { get; set; }
    }

    public class BuyTokensDTO
    {
        public string Address { get; set; }
        public decimal Amount { get; set; }
    }

    public class BuyingReceiptDTO
    {
        public decimal AmountBought { get; set; }
        public decimal Value { get; set; }
        public TransactionReceiptDTO TransactionReceipt { get; set; }
        public DateTime PurchaseDate { get; set; }
    }

    public class PurchaseDTO
    {
        public BuyingReceiptDTO ActualPurchase { get; set; }
    }

    public class TransactionReceiptDTO
    {
        public string TransactionHash { get; set; }
        public string TransactionIndex { get; set; }
        public string BlockHash { get; set; }
        public string BlockNumber { get; set; }
        public string CumulativeGasUsed { get; set; }
        public string GasUsed { get; set; }
        public string ContractAddress { get; set; }
        public string Status { get; set; }
        public LogDTO[] Logs { get; set; }
    }

    public class LogDTO
    {
        public string Address { get; set; }
        public string[] Topics { get; set; }
        public string Data { get; set; }
        public string BlockNumber { get; set; }
        public string TransactionHash { get; set; }
        public string TransactionIndex { get; set; }
        public string BlockHash { get; set; }
        public string LogIndex { get; set; }
        public bool Removed { get; set; }
    }

    public class AirdropSubscriptionDTO
    {
        public AirdropDefinitionDTO AirdropDefinition { get; set; }
        public int Count { get; set; }
    }

    public class AirdropDefinitionDTO
    {
        public int Id { get; set; }
        public string AirdropName { get; set; }
        public string TokenName { get; set; }
        public int Amount { get; set; }
        public int MaxLimit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string OtherInfoToDisplay { get; set; }
        public TwitterAirdropRequirementDTO TwitterAirdropRequirement { get; set; }
        public TelegramAirdropRequirementDTO TelegramAirdropRequirement { get; set; }
        public BitcoinTalkAirdropRequirementDTO BitcoinTalkAirdropRequirement { get; set; }
    }

    public class UserAirdropDTO
    {
        public int UserId { get; set; }
        public List<int> AirdropIds { get; set; }
    }

    public class AirdropSubscriptionResultDTO
    {
        public UserAirdropDTO UserAirdrops { get; set; }
        public bool Success { get; set; }
        public FailReason FailReason { get; set; }
    }

    public class TwitterAirdropRequirementDTO
    {
        public bool HasAccount { get; set; }
        public bool HasAccountApplies { get; set; }
        public int MinimumFollowers { get; set; }
        public bool MinimumFollowersApplies { get; set; }
        public DateTime? MinimumCreationDate { get; set; }
        public bool MinimumCreationDateApplies { get; set; }
    }

    public class TelegramAirdropRequirementDTO
    {
        public bool HasAccount { get; set; }
        public bool HasAccountApplies { get; set; }
    }

    public class BitcoinTalkAirdropRequirementDTO
    {
        public bool HasAccount { get; set; }
        public bool HasAccountApplies { get; set; }
        public int MinimumPosts { get; set; }
        public bool MinimumPostsApplies { get; set; }
        public int MinimumActivity { get; set; }
        public bool MinimumActivityApplies { get; set; }
        public BitcoinTalkRank? MinimumRank { get; set; }
        public bool MinimumRankApplies { get; set; }
        public BitcoinTalkRank? ExactRank { get; set; }
        public bool ExactRankApplies { get; set; }
        public DateTime? MinimumCreationDate { get; set; }
        public bool MinimumCreationDateApplies { get; set; }
    }

    public class SendFundsDTO
    {
        public string ToAddress { get; set; }
        public string FromAddress { get; set; }
        public decimal Eth { get; set; }
    }

    public class WithdrawalReceiptDTO
    {
        public string TransactionHash { get; set; }
    }

    public class DocumentVersion
    {
        public string Filename { get; set; }
        public DocumentType DocumentType { get; set; }
        public bool DocumentAvailable { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public FileType FileType { get; set; }
    }
}
