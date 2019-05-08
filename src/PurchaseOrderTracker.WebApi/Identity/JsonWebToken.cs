using System;
using Newtonsoft.Json;

namespace PurchaseOrderTracker.WebApi.Identity
{
    public class JsonWebToken
    {
        public JsonWebToken(string accessToken, double expiresIn, string refreshToken)
        {
            AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
            RefreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken));
            ExpiresIn = expiresIn;
        }

        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; }

        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; }

        [JsonProperty(PropertyName = "expires_in")]
        public double ExpiresIn { get; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType => "Bearer";
    }
}
