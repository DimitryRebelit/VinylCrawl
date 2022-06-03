using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinylCrawl.Discogs.Agents.Interfaces
{
    internal interface IIdentityAgent
    {
        Task <string> GetIdentity();
    }
}
