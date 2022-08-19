using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Coinstantine.Data
{
    public class Translation : IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Key { get; set; }
        public string Value { get; set; }
        public string Language { get; set; }
    }

    public class Price : IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public decimal Usd { get; set; }
        public decimal Eur { get; set; }
    }

    public class CoinstantinePriceConfig : IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public decimal Eth { get; set; }
        public int Bonus { get; set; }
        public decimal MinimumEth { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class SmartContractDefinition : IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Abi { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
    }

    public class BuyingReceipt : IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public decimal AmountBought { get; set; }
        public decimal Value { get; set; }
        public string TransactionHash { get; set; }
        public string CumulativeGasUsed { get; set; }
        public string GasUsed { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int UserId { get; set; }
    }

    public class Purchase
    {
        public BuyingReceipt ActualPurchase { get; set; }
    }

    public class EnvironmentInfo
    {
        public string EtherscanUrl { get; set; }
        public string Web3Url { get; set; }
        public ApiEnvironment ApiEnvironment { get; set; }
        public EthereumNetwork EthereumNetwork { get; set; }
    }

    public enum ApiEnvironment
    {
        Localhost,
        Production,
        Preprod,
        Acceptance
    }

    public enum EthereumNetwork
    {
        Ropsten,
        Rinkeby,
        Mainnet
    }

    public class AirdropDefinition : IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int AirdropId { get; set; }
        public string AirdropName { get; set; }
        public string TokenName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string OtherInfoToDisplay { get; set; }
        public int Amount { get; set; }
        public int NumberOfSubscribers { get; set; }
        public int MaxLimit { get; set; }

        [ForeignKey(typeof(TwitterAirdropRequirement))]
        public int? TwitterAirdropRequirementId { get; set; }
        [ForeignKey(typeof(TelegramAirdropRequirement))]
        public int? TelegramAirdropRequirementId { get; set; }
        [ForeignKey(typeof(BitcoinTalkAirdropRequirement))]
        public int? BitcoinTalkAirdropRequirementId { get; set; }

        [OneToOne(nameof(TwitterAirdropRequirementId), CascadeOperations = CascadeOperation.All)]
        public TwitterAirdropRequirement TwitterAirdropRequirement { get; set; }
        [OneToOne(nameof(TelegramAirdropRequirementId), CascadeOperations = CascadeOperation.All)]
        public TelegramAirdropRequirement TelegramAirdropRequirement { get; set; }
        [OneToOne(nameof(BitcoinTalkAirdropRequirementId), CascadeOperations = CascadeOperation.All)]
        public BitcoinTalkAirdropRequirement BitcoinTalkAirdropRequirement { get; set; }
    }

    public class UserAirdrop : IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int UserId { get; set; }
        [TextBlob("Ids")]
        public List<int> AidropDefinitionIds { get; set; }
        public string Ids { get; set; }
    }

    public class AirdropSubscriptionResult
    {
        public UserAirdrop UserAirdrops { get; set; }
        public bool Success { get; set; }
        public FailReason FailReason { get; set; }
    }

    public enum FailReason
    {
        None,
        Full,
        RequirementsNotMet,
        UnknownAirdrop,
        AlreadySubscribed,
        Expired,
        NotStarted
    }

    public interface IAirdropRequirement 
    {
        
    }
    public class TwitterAirdropRequirement : IEntity, IAirdropRequirement
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public bool HasAccount { get; set; }
        public bool HasAccountApplies { get; set; }

        public int MinimumFollowers { get; set; }
        public bool MinimumFollowersApplies { get; set; }

        public DateTime? MinimumCreationDate { get; set; }
        public bool MinimumCreationDateApplies { get; set; }
    }

    public class TelegramAirdropRequirement : IEntity, IAirdropRequirement
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public bool HasAccount { get; set; }
        public bool HasAccountApplies { get; set; }
    }

    public class BitcoinTalkAirdropRequirement : IEntity, IAirdropRequirement
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public bool HasAccount { get; set; }
        public bool HasAccountApplies { get; set; }

        public int MinimumPosts { get; set; }
        public bool MinimumPostsApplies { get; set; }

        public int MinimumActivity { get; set; }
        public bool MinimumActivityApplies { get; set; }

        public string MinimumRank { get; set; }
        public BitcoinTalkRank? MinimumPosition { get; set; }
        public bool MinimumRankApplies { get; set; }

        public string ExactRank { get; set; }
        public BitcoinTalkRank? ExactPosition { get; set; }
        public bool ExactRankApplies { get; set; }

        public DateTime? MinimumCreationDate { get; set; }
        public bool MinimumCreationDateApplies { get; set; }
    }
}
