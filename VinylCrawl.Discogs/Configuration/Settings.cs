namespace VinylCrawl.Discogs.Configuration
{
    public static class Settings
    {
        /// <summary>
        ///     Consumer key provided by Discogs to get a higher tier rate limiting
        /// </summary>
        public static string ConsumerKey => "msjwUQJBgaUGpeTZXPVI";

        /// <summary>
        ///     Consumer secret provided by Discogs to get a higher tier rate limiting
        /// </summary>
        public static string ConsumerSecret => "RxHHbMkDoDoNKqtWRpNWSLeMiRLJbGNs";

        public static string CallbackUri => "vinylcrawl://";

        public static string BaseUrl => "https://api.discogs.com/";
    }
}