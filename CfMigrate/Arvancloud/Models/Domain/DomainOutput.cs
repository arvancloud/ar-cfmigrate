using System.Text.Json.Serialization;

namespace CfMigrate.Arvancloud.Models.Domain
{
    public class DomainOutput
    {
        public string Id { get; set; }
        
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }
        public string Domain { get; set; }
        public string Name { get; set; }
    }
}