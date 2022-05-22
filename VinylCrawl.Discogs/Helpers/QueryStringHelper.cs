using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylCrawl.Discogs.Helpers
{
    public static class QueryStringHelper
    {
        public static Dictionary<string, string> ParseQueryString(string requestQueryString)
        {
            Dictionary<string, string> rc = new Dictionary<string, string>();
            string[] ar1 = requestQueryString.Split(new char[] { '&', '?' });
            foreach (string row in ar1)
            {
                if (string.IsNullOrEmpty(row)) continue;
                int index = row.IndexOf('=');
                if (index < 0) continue;
                rc[Uri.UnescapeDataString(row.Substring(0, index))] = Uri.UnescapeDataString(row.Substring(index + 1)); // use Unescape only parts          
            }
            return rc;
        }
    }
}
