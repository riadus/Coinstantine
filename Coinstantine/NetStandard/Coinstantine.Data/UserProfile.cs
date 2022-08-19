using System;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Coinstantine.Data
{
	public interface IEntity
    {
        int Id { get; set; }
    }

	public class UserProfile : IEntity
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public string Username { get; set; }
		public string Email { get; set; }

		[ForeignKey(typeof(PhonenumberProfile))]
		public int? PhonenumberProfileId { get; set; }
		[ForeignKey(typeof(TelegramProfile))]
		public int? TelegramProfileId { get; set; }
		[ForeignKey(typeof(TwitterProfile))]
		public int? TwitterProfileId { get; set; }
		[ForeignKey(typeof(BitcoinTalkProfile))]
		public int? BitcoinTalkProfileId { get; set; }
        [ForeignKey(typeof(BlockchainInfo))]
        public int? BlockchainInfoId { get; set; }

		[OneToOne(nameof(PhonenumberProfileId), CascadeOperations = CascadeOperation.All)]
		public PhonenumberProfile PhonenumberProfile { get; set; }
		[OneToOne(nameof(TelegramProfileId), CascadeOperations = CascadeOperation.All)]
		public TelegramProfile TelegramProfile { get; set; }
		[OneToOne(nameof(TwitterProfileId), CascadeOperations = CascadeOperation.All)]
		public TwitterProfile TwitterProfile { get; set; }
		[OneToOne(nameof(BitcoinTalkProfileId), CascadeOperations = CascadeOperation.All)]
		public BitcoinTalkProfile BitcoinTalkProfile { get; set; }

        [OneToOne(nameof(BlockchainInfoId), CascadeOperations = CascadeOperation.All)]
        public BlockchainInfo BlockchainInfo { get; set; }

		public string DataAccesSalt { get; set; }
		public bool LoggedIn { get; set; }
		public int InvalidPincodeAttempts { get; set; }
		public bool NeedsReset { get; set; }
		public bool UseBiometricsToLogin { get; set; }
		public string PinCode { get; set; }
		public DateTime? LastUserSession { get; set; }
		public DateTime? LastSyncSession { get; set; }
		public string PreferredLanguage { get; set; }
	}

    public class PhonenumberProfile : IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Phonenumber { get; set; }
        public bool Validated { get; set; }
    }

    public interface IProfileItem {
        string Username { get; set; }
        bool Validated { get; set; }
        DateTime? ValidationDate { get; set; }
    }

    public class TwitterProfile : IEntity, IProfileItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

		public string InAppUsername { get; set; }
        public string ScreenName { get; set; }
        public string Username { get; set; }
        public int Followers { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Validated { get; set; }
        public long TwitterId { get; set; }
        public long TweetId { get; set; }
        public DateTime? ValidationDate { get; set; }
    }

    public class TelegramProfile : IEntity, IProfileItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

		public string InAppUsername { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phonenumber { get; set; }
        public long TelegramId { get; set; }
        public bool Validated { get; set; }
        public DateTime? ValidationDate { get; set; }
    }

    public class BitcoinTalkProfile : IEntity, IProfileItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

		public string InAppUserId { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Rank { get; set; }
        public BitcoinTalkRank Position { get; set; }
        public string Location { get; set; }
        public DateTime RegistredDate { get; set; }
        public int Posts { get; set; }
        public int Activity { get; set; }
        public bool Validated { get; set; }
        public DateTime? ValidationDate { get; set; }
    }

    public class BlockchainInfo : IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public decimal Ether { get; set; }
        public decimal Coinstantine { get; set; }
        public string Address { get; set; }
    }

    public class Document : IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Ignore]
        public byte[] Bytes { get; set; }
        public string Filename { get; set; }
        public DocumentType DocumentType { get; set; }
        public bool DocumentAvailable { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string Path { get; set; }
        public FileApplicationType FileType { get; set; }
    }

    public class FileType
    {
        public FileApplicationType ApplicationType { get; set; }
        public string ApplicationTypeString { get; set; }
    }

    public enum DocumentType
    {
        WhitePaper,
        TermsAndServices,
        PrivacyPolicy
    }

    public enum FileApplicationType
    {
        Web,
        PDF,
        Word,
        Excel,
        Other
    }
}
