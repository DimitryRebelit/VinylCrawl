using System.Text.Json.Serialization;

namespace VinylCrawl.Discogs.Models
{
    public class Identity
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("resource_url")]
        public string ResourceUrl { get; set; }

        [JsonPropertyName("consumer_name")]
        public string ConsumerName { get; set; }
    }
}
