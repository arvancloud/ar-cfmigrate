using System;
using System.Text.Json.Serialization;

namespace CloudFlareLib.Models.Dns
{
    public class DnsOutput
    {
        public string Id { get; set; }
        
        [JsonPropertyName("zone_id")]
        public string ZoneId { get; set; }
        
        [JsonPropertyName("zone_name")]
        public string ZoneName { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public bool Proxiable { get; set; }
        public bool Proxied { get; set; }
        public int Ttl { get; set; }
        public bool Locked { get; set; }
        public Meta Meta { get; set; }
        
        [JsonPropertyName("created_on")]
        public DateTime CreatedOn { get; set; }
        
        [JsonPropertyName("modified_on")]
        public DateTime ModifiedOn { get; set; }
    }

    public class Meta
    {
        [JsonPropertyName("auto_added")]
        public bool AutoAdded { get; set; }
        
        [JsonPropertyName("managed_by_apps")]
        public bool ManagedByApps { get; set; }
        
        [JsonPropertyName("managed_by_argo_tunnel")]
        public bool ManagedByArgoTunnel { get; set; }
        
        public string Source { get; set; }
    }
}