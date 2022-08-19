namespace Coinstantine.Domain.Auth.Models
{
    public class AccountChangeModel
    {
        public string UserId { get; set; }
        public string ConfirmationCode { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
