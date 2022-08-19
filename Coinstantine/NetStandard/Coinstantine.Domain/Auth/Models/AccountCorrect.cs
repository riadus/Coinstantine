namespace Coinstantine.Domain.Auth.Models
{
    public class AccountCorrect
    {
        public bool UsernameAvailable { get; set; }
        public bool EmailAvailable { get; set; }
        public bool PasswordCorrect { get; set; }
        public bool AllGood { get; set; }
    }
}
