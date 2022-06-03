using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinylCrawl.Discogs.Agents.Interfaces;

namespace VinylCrawl.Discogs.Agents
{
    internal class IdentityAgent : BaseHttpAgent, IIdentityAgent
    {
        public async Task<string> GetIdentity()
        {
            var uri = "oauth/identy";
            return await GetWithAuthAsync<string>(uri);
        }
    }
}
