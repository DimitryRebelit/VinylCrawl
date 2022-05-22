using VinylCrawl.Discogs.Agents.Interfaces;
using VinylCrawl.Discogs.Models;
using VinylCrawl.Discogs.Responses;

namespace VinylCrawl.Discogs.Agents
{
    public class AuthAgent : BaseHttpAgent, IAuthAgent
    {
        public async Task<RetrieveRequestTokenResponse?> RetrieveRequestToken(string consumerKey, string consumerSecret)
        {
            var uri = "oauth/request_token";
            var authorizationHeader = 
                $"oauth_consumer_key=\"{consumerKey}\"," +
                $"oauth_signature_method=\"PLAINTEXT\"," +
                $"oauth_timestamp=\"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}\"," +
                $"oauth_nonce=\"{Guid.NewGuid()}\"," +
                $"oauth_version=\"1.0\"," +
                $"oauth_signature=\"{consumerSecret}&\", " +
                $"oauth_callback=\"vinylcrawl://\"";

            return await RequestTokenOAuth(uri,authorizationHeader);
        }

        public async Task<RetrieveRequestTokenResponse?> RetrieveAccessToken(string consumerKey, string consumerSecret, string oAuthVerifier, string oAuthToken, string oAuthTokenSecret)
        {
            try 
            {
                var uri = "oauth/access_token";
                var authorizationHeader =
                    $"oauth_consumer_key=\"{consumerKey}\"," +
                    $"oauth_nonce=\"{Guid.NewGuid()}\"," +
                    $"oauth_token=\"{oAuthToken}\"," +
                    $"oauth_signature=\"{consumerSecret}&{oAuthTokenSecret}\"," +
                    $"oauth_signature_method=\"PLAINTEXT\"," +
                    $"oauth_timestamp=\"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}\"," +
                    $"oauth_verifier=\"{oAuthVerifier}\"";

                /*
                OAuth oauth_consumer_key="your_consumer_key",
                oauth_nonce="random_string_or_timestamp",
                oauth_token="oauth_token_received_from_step_2"
                oauth_signature="your_consumer_secret&",
                oauth_signature_method="PLAINTEXT",
                oauth_timestamp="current_timestamp",
                oauth_verifier="users_verifier"
                */

                return await AccessTokenOAuth(uri, authorizationHeader);
            }
            catch(Exception e)
            {
                throw;
            }
        }

        public async Task<Identity> GetIdentity()
        {
            try
            {
                var uri = "oauth/identity";
                return await GetWithAuthAsync<Identity>(uri);
            }
            catch(Exception e)
            {
                throw;
            }
        }
    }
}
