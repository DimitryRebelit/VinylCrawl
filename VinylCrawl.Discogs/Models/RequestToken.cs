using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylCrawl.Discogs.Models
{
    public class OAuthToken
    {
        public string Token { get; set; }
        public string TokenSecret { get; set; }
        public string AuthCallbackConfirmed { get; set; }
    }
}
