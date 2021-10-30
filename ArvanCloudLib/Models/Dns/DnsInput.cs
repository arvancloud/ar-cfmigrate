using System.Text.Json.Serialization;

namespace ArvanCloudLib.Models.Dns
{
    public class DnsInput
    {
        public string Domain { get; set; }
        public DnsType Type { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
        public DnsTtl Ttl { get; set; }
        public bool Cloud { get; set; }
        
        [JsonPropertyName("upstream_https")]
        public DnsUpStreamHttps UpStreamHttps { get; set; }
    }
}