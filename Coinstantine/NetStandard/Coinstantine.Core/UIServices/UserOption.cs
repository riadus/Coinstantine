namespace Coinstantine.Core.UIServices
{
    public class UserOption<T>
    {
        public UserOption(T option, string text)
        {
            Value = option;
            Label = text;
        }

        public T Value { get; }
        public string Label { get; }
    }
}
