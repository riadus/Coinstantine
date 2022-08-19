namespace Coinstantine.Domain.Airdrops
{
    public class Condition<T>
    {
        public string Name { get; set; }
        public bool Applies { get; set; }
        public T ConditionToMatch { get; set; }
    }
}
