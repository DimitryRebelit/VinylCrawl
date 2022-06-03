using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinylCrawl.Discogs.Configuration;
using VinylCrawl.Discogs.Services.Interfaces;

namespace VinylCrawl.ViewModels
{
    public class LoginViewModel
    {
        private readonly IDiscogsService _discogsService;

        public LoginViewModel(IDiscogsService discogsService)
        {
            _discogsService = discogsService;
        }

        public async Task LoginButtonPressed(object sender, EventArgs e)
        {
            // Get the token so we can talk to the API as a registered application
            var token = await _discogsService.RetrieveRequestTokenAsync(Settings.ConsumerKey, Settings.ConsumerSecret);
            var uri = _discogsService.GenerateAuthenticateUri(token);

            var webAuthenticatorUrl = await WebAuthenticator.AuthenticateAsync(uri, new Uri(Settings.CallbackUri));
            var oAuthToken = webAuthenticatorUrl.Properties["oauth_token"];
            var oAuthverifier = webAuthenticatorUrl.Properties["oauth_verifier"];
            var accessToken = await _discogsService.RetrieveAccessToken(Settings.ConsumerKey, Settings.ConsumerSecret, oAuthverifier, oAuthToken, token.TokenSecret);

            // Set the token for the user
            await SecureStorage.SetAsync("oauth_token", accessToken.Token);
            await SecureStorage.SetAsync("oauth_secret", accessToken.TokenSecret);
        }
    }
}
