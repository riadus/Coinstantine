using System.Collections.Generic;
using Newtonsoft.Json;

namespace Coinstantine.Domain.Auth
{
    public class JsonWebToken
    {
        [JsonProperty("iss")]
        public string Issuer { get; set; }

        [JsonProperty("scp")]
        public string Scopes { get; set; }

        [JsonProperty("exp")]
        public long Expiry { get; set; }

        [JsonProperty("iat")]
        public long IssuedAt { get; set; }

        [JsonProperty("nbf")]
        public string NotBefore { get; set; }

        [JsonProperty("aud")]
        public string Audience { get; set; }

        [JsonProperty("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")]
        public string NameIdentifier { get; set; }

        [JsonProperty("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")]
        public string Email { get; set; }

        [JsonProperty("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")]
        public string Role { get; set; }
    }
}
