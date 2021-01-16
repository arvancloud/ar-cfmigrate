namespace CfMigrate.Cloudflare.Models.Dns
{
    public class ShortDnsOutput
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Ttl { get; set; }
    }
}