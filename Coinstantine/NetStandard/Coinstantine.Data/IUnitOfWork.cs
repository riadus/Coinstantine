namespace Coinstantine.Data
{
    public interface IUnitOfWork
    {
        IRepository<UserProfile> UserProfiles { get; }
        IRepository<TwitterProfile> TwitterProfiles { get; }
        IRepository<TelegramProfile> TelegramProfiles { get; }
        IRepository<BitcoinTalkProfile> BitcoinTalkProfiles { get; }
		IRepository<AuthenticationObject> AuthenticationObjects { get; }
        IRepository<Setting> Settings { get; }
        IRepository<Translation> Translations { get; }
        IRepository<CoinstantinePriceConfig> CoinstantinePriceConfigs { get; }
        IRepository<Price> Prices { get; }
        IRepository<SmartContractDefinition> SmartContractDefinitions { get; }
        IRepository<BuyingReceipt> BuyingReceipts { get; }
        IRepository<AirdropDefinition> AirdropDefinitions { get; }
        IRepository<UserAirdrop> UserAirdrops { get; }
        IRepository<Document> Documents { get; }
    }
}
