using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinylCrawl.Discogs.Helpers;

namespace VinylCrawl.Discogs.Responses
{
    internal class RetrieveRequestTokenResponse
    {
        public RetrieveRequestTokenResponse(string response)
        {
            var parsedQueryString = QueryStringHelper.ParseQueryString(response);
            OAuthToken = parsedQueryString["oauth_token"];
            OAuthTokenSecret = parsedQueryString["oauth_token_secret"];
            
            if(parsedQueryString.ContainsKey("oauth_callback_confirmed"))
                OAuthCallbackConfirmed = parsedQueryString["oauth_callback_confirmed"];
        }

        public string OAuthToken { get; set; }

        public string OAuthTokenSecret { get; set; }

        public string OAuthCallbackConfirmed { get; set; }
    }
}
