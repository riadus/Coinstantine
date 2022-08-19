using Coinstantine.Domain.Interfaces.Translations;

namespace Coinstantine.Domain.Airdrops
{
    public class ItemInfo
    {
        public TranslationKey Title { get; set; }
        public string Value { get; set; }
        public Display Display { get; set; }

        public static implicit operator ItemInfo((TranslationKey Title, string Value, Display Display) value)
        {
            return new ItemInfo
            {
                Title = value.Title,
                Value = value.Value,
                Display = value.Display
            };
        }

        public static implicit operator ItemInfo((TranslationKey Title, string Value) value)
        {
            return new ItemInfo
            {
                Title = value.Title,
                Value = value.Value,
                Display = Display.New
            };
        }
    }

    public enum Display
    {
        New,
        Grouped,
        Title,
    }
}
