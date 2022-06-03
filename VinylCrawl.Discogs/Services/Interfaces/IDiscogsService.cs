using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinylCrawl.Discogs.Models;

namespace VinylCrawl.Discogs.Services.Interfaces
{
    public interface IDiscogsService
    {
        /// <summary>
        ///     Retrieves a request token needed to communicate as a application to Discogs
        /// </summary>
        /// <param name="consumerKey">Key registered in Discogs for the application</param>
        /// <param name="consumerSecret">Secret that Discogs has assigned to the key</param>
        /// <returns>A string containing a token. This can be used in headers or to generate a url so the user can login</returns>
        public Task<OAuthToken> RetrieveRequestTokenAsync(string consumerKey, string consumerSecret);

        /// <summary>
        ///     Retrieves a access token for the user to interact with the api
        /// </summary>
        /// <param name="consumerKey">Key registered in Discogs for the application</param>
        /// <param name="consumerSecret">Secret that Discogs has assigned to the key</param>
        /// <param name="oAuthVerifier">Verifier that is returned after user has logged in</param>
        /// <param name="oAuthToken">Token that is returend after the user has logged in</param>
        /// <param name="oAuthTokenSecret">Secret from the Request Token</param>
        /// <returns></returns>
        /// <remarks>
        ///     The oAuth parameters are retrieved by letting a user signing with OAuth in a browser. If you use .NET MAUI you can use WebAuthenticator for example to open a browser and let the user login. 
        /// </remarks>
        public Task<OAuthToken> RetrieveAccessToken(string consumerKey, string consumerSecret, string oAuthVerifier, string oAuthToken, string oAuthTokenSecret);

        /// <summary>
        ///     Generate a authorization url used to login the user
        /// </summary>
        /// <param name="requestToken"></param>
        /// <returns></returns>
        public Uri GenerateAuthenticateUri(OAuthToken requestToken);    
    }
}
