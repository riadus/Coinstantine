using System;
namespace Coinstantine.Data
{
    public class TwitterProfileOnline
    {
        public string ScreenName { get; set; }
        public long TwitterId { get; set; }
        public int NumberOfFollower { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Validated { get; set; }
        public DateTime? ValidationDate { get; set; }
    }

	public class TelegramProfileOnline
    {
        public string Username { get; set; }
        public long TelegramId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Validated { get; set; }
        public DateTime? ValidationDate { get; set; }
    }
}
