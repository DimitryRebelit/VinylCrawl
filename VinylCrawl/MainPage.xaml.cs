using VinylCrawl.Discogs.Agents;
using VinylCrawl.Discogs.Agents.Interfaces;
using VinylCrawl.Discogs.Configuration;

namespace VinylCrawl;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class MainPage : ContentPage
{

    public MainPage()
    {
        InitializeComponent();
    }

    private async void LinkDiscogsButton_Clicked(object sender, EventArgs e)
    {
        var identityAgent = DependencyService.Resolve<IIdentityAgent>();
        var collectionAgent = DependencyService.Resolve<ICollectionAgent>();
        var authAgent = DependencyService.Resolve<IAuthAgent>();

        if (string.IsNullOrEmpty(await SecureStorage.GetAsync("oauth_token")) || string.IsNullOrEmpty(await SecureStorage.GetAsync("oauth_secret")))
        {
            var token = await authAgent.RetrieveRequestToken(Settings.ConsumerKey, Settings.ConsumerSecret);

            var authResult = await WebAuthenticator.AuthenticateAsync(new Uri($"https://discogs.com/oauth/authorize?oauth_token={token.OAuthToken}"), new Uri("vinylcrawl://"));
            var oAuthToken = authResult.Properties["oauth_token"];
            var oAuthverifier = authResult.Properties["oauth_verifier"];

            var accessToken = await authAgent.RetrieveAccessToken(Settings.ConsumerKey, Settings.ConsumerSecret, oAuthverifier, oAuthToken, token.OAuthTokenSecret);

            // Set the token for the user
            await SecureStorage.SetAsync("oauth_token", accessToken.OAuthToken);
            await SecureStorage.SetAsync("oauth_secret", accessToken.OAuthTokenSecret);
        }

        var x = await authAgent.GetIdentity();
        var t = await collectionAgent.GetCollection(x.Username);



    }
}

