using System.Text;
using System.Threading.Tasks;

namespace VinylCrawl.Discogs.Agents.Interfaces
{
    /// <summary>
    ///     Documentation
    /// </summary>
    internal interface ICollectionAgent
    {
        public Task<string> GetCollection(string userName);
    }
}
