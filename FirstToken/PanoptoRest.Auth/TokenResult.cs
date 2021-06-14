using System;
using Newtonsoft.Json;

namespace PanoptoRest.Auth
{
    public partial class TokenResult
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }

        public DateTime? Created;
        public DateTime? Expires;

        public bool IsValidOnCreation()
        {
            return string.IsNullOrEmpty(Error) && !string.IsNullOrEmpty(AccessToken) && ExpiresIn > 0;
        }

        public bool IsValidNow()
        {
            return (IsValidOnCreation() && Expires.HasValue && Expires.Value < DateTime.Now);
        }
    }
}
