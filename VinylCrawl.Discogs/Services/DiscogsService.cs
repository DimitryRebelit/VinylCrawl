using VinylCrawl.Discogs.Agents.Interfaces;
using VinylCrawl.Discogs.Models;
using VinylCrawl.Discogs.Services.Interfaces;

namespace VinylCrawl.Discogs.Services
{
    ///<inheritdoc />
    internal class DiscogsService : IDiscogsService
    {
        private readonly IIdentityAgent _identityAgent;
        private readonly IAuthAgent _authAgent;
        private readonly ICollectionAgent _collectionAgent;

        /// <summary>
        ///     Default Constructor
        /// </summary>
        /// <param name="identityAgent"></param>
        /// <param name="authAgent"></param>
        /// <param name="collectionAgent"></param>
        public DiscogsService(IIdentityAgent identityAgent, IAuthAgent authAgent, ICollectionAgent collectionAgent)
        {
            _identityAgent = identityAgent;
            _authAgent = authAgent;
            _collectionAgent = collectionAgent;
        }

        ///<inheritdoc />
        public Uri GenerateAuthenticateUri(OAuthToken requestToken) => new Uri($"https://discogs.com/oauth/authorize?oauth_token={requestToken.Token}");

        ///<inheritdoc />
        public async Task<OAuthToken> RetrieveAccessToken(string consumerKey, string consumerSecret, string oAuthVerifier, string oAuthToken, string oAuthTokenSecret)
        {
            var accessToken = await _authAgent.RetrieveAccessToken(consumerKey, consumerSecret, oAuthVerifier, oAuthToken, oAuthTokenSecret);
            return new OAuthToken
            {
                Token = accessToken.OAuthToken,
                TokenSecret = accessToken.OAuthTokenSecret,
                AuthCallbackConfirmed = accessToken.OAuthCallbackConfirmed
            };
        }

        ///<inheritdoc />
        public async Task<OAuthToken> RetrieveRequestTokenAsync(string consumerKey, string consumerSecret)
        {
            var token = await _authAgent.RetrieveRequestToken(consumerKey, consumerSecret);
            return new OAuthToken
            {
                Token = token.OAuthToken,
                TokenSecret = token.OAuthTokenSecret,
                AuthCallbackConfirmed = token.OAuthCallbackConfirmed
            };
        }
    }
}
