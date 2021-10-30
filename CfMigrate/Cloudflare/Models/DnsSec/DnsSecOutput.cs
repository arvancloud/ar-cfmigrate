using System;
using System.Text.Json.Serialization;

namespace CfMigrate.Cloudflare.Models.DnsSec
{
    public class DnsSecOutput
    {
        public string Status { get; set; }
        public int Flag { get; set; }
        public string Algorithm { get; set; }
        
        [JsonPropertyName("key_type")]
        public string KeyType { get; set; }
        
        [JsonPropertyName("digest_algorithm")]
        public string DigestAlgorithm { get; set; }

        public string Digest { get; set; }
        public string Ds { get; set; }
        
        [JsonPropertyName("key_tag")]
        public int KeyTag { get; set; }

        [JsonPropertyName("public_key")]
        public string PublicKey { get; set; }

        [JsonPropertyName("modified_on")]
        public DateTime ModifiedOn { get; set; }
    }
}