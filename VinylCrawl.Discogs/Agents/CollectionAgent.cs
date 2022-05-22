using System.Text;
using System.Threading.Tasks;
using VinylCrawl.Discogs.Agents.Interfaces;

namespace VinylCrawl.Discogs.Agents
{
    public class CollectionAgent : BaseHttpAgent, ICollectionAgent
    {
        public async Task<string> GetCollection(string userName)
        {
            var url = $"users/{userName}/collection/folders/0/releases";
            return  await GetWithAuthAsync<string>(url);
        }
    }
}
