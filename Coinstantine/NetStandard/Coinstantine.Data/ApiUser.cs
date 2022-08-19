namespace Coinstantine.Data
{
    public class ApiUser
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Phonenumber { get; set; }
        public string Culture { get; set; }
        public string DeviceId { get; set; }
        public string UniqueId { get; set; }
        public TwitterProfileOnline TwitterProfile { get; set; }
        public ThirdPartyValidationOnline Facebook { get; set; }
        public TelegramProfileOnline Telegram { get; set; }
        public BitcoinTalkUserOnline BctProfile { get; set; }
        public BlockchainInfo BlockchainInfo { get; set; }
    }
}
