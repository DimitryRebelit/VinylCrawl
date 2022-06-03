using VinylCrawl.Discogs.Models;
using VinylCrawl.Discogs.Responses;

namespace VinylCrawl.Discogs.Agents.Interfaces
{
    /// <summary>
    ///     Authentication agent for communication with the authentication endpoints of the discogs api
    /// </summary>
    internal interface IAuthAgent
    {
        /// <summary>
        ///     Get request token for the app
        /// </summary>
        /// <param name="consumerKey"></param>
        /// <param name="consumerSecret"></param>
        /// <remarks>This token is used to send request to the api as a registered application. This disables the low tier rate limit</remarks>
        /// <returns></returns>
        public Task<RetrieveRequestTokenResponse?> RetrieveRequestToken(string consumerKey, string consumerSecret);

        public Task<RetrieveRequestTokenResponse?> RetrieveAccessToken(string consumerKey, string consumerSecret, string oAuthVerifier, string oAuthToken, string oAuthTokenSecret);


        Task<Identity> GetIdentity();
    }
}
