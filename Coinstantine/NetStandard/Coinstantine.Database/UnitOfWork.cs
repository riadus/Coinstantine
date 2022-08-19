using Coinstantine.Common.Attributes;
using Coinstantine.Data;

namespace Coinstantine.Database
{
    [RegisterInterfaceAsLazySingleton]
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork()
        {
            var connection = SqliteDatabase.GetAsyncConnection(SqliteDatabase.ConnectionType.First);
            var secondConnection = SqliteDatabase.GetAsyncConnection(SqliteDatabase.ConnectionType.Second);
            var thirdConnection = SqliteDatabase.GetAsyncConnection(SqliteDatabase.ConnectionType.Third);

            UserProfiles = new AsyncRepository<UserProfile>(secondConnection);
            TwitterProfiles = new AsyncRepository<TwitterProfile>(secondConnection);
            TelegramProfiles = new AsyncRepository<TelegramProfile>(secondConnection);
            BitcoinTalkProfiles = new AsyncRepository<BitcoinTalkProfile>(secondConnection);

            AuthenticationObjects = new AsyncCachedRepository<AuthenticationObject>(thirdConnection);

            Settings = new AsyncRepository<Setting>(connection);
            Translations = new AsyncCachedRepository<Translation>(connection);
            CoinstantinePriceConfigs = new AsyncCachedRepository<CoinstantinePriceConfig>(connection);
            Prices = new AsyncCachedRepository<Price>(connection);
            SmartContractDefinitions = new AsyncCachedRepository<SmartContractDefinition>(connection);
            BuyingReceipts = new AsyncCachedRepository<BuyingReceipt>(connection);
            AirdropDefinitions = new AsyncCachedRepository<AirdropDefinition>(connection);
            UserAirdrops = new AsyncRepository<UserAirdrop>(connection);
            Documents = new AsyncRepository<Document>(connection);
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
