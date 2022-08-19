using Coinstantine.Data;

namespace Coinstantine.UnitTests
{
    public class MockUnitOfWork : IUnitOfWork
    {
        public MockUnitOfWork()
        {
            UserProfiles = new MockRepository<UserProfile>();
            AuthenticationObjects = new MockRepository<AuthenticationObject>();
            Settings = new MockRepository<Setting>();
            Translations = new MockRepository<Translation>();
            CoinstantinePriceConfigs = new MockRepository<CoinstantinePriceConfig>();
            Prices = new MockRepository<Price>();
            SmartContractDefinitions = new MockRepository<SmartContractDefinition>();
            BuyingReceipts = new MockRepository<BuyingReceipt>();
            AirdropDefinitions = new MockRepository<AirdropDefinition>();
            UserAirdrops = new MockRepository<UserAirdrop>();
            TwitterProfiles = new MockRepository<TwitterProfile>();
            TelegramProfiles = new MockRepository<TelegramProfile>();
            BitcoinTalkProfiles = new MockRepository<BitcoinTalkProfile>();
            Documents = new MockRepository<Document>();
        }

        public IRepository<UserProfile> UserProfiles { get; }

        public IRepository<AuthenticationObject> AuthenticationObjects { get; }

        public IRepository<Setting> Settings { get; }

        public IRepository<Translation> Translations { get; }

        public IRepository<CoinstantinePriceConfig> CoinstantinePriceConfigs { get; }

        public IRepository<Price> Prices { get; }

        public IRepository<SmartContractDefinition> SmartContractDefinitions { get; }

        public IRepository<BuyingReceipt> BuyingReceipts { get; }

        public IRepository<AirdropDefinition> AirdropDefinitions { get; }

        public IRepository<UserAirdrop> UserAirdrops { get; }

        public IRepository<TwitterProfile> TwitterProfiles { get; }

        public IRepository<TelegramProfile> TelegramProfiles { get; }

        public IRepository<BitcoinTalkProfile> BitcoinTalkProfiles { get; }

        public IRepository<Document> Documents { get; }
}
}
