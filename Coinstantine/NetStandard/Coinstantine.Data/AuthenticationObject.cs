using System;
using SQLite;

namespace Coinstantine.Data
{
    public class AuthenticationObject : IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime RefreshTokenExpirationDate { get; set; }
        public string NameIdentifier { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
