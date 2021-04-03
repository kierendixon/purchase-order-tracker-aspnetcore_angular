using System;
using System.Text.Json.Serialization;

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

        [JsonPropertyName("access_token")]
        public string AccessToken { get; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; }

        [JsonPropertyName("expires_in")]
        public double ExpiresIn { get; }

        [JsonPropertyName("token_type")]
        public string TokenType => "Bearer";
    }
}
