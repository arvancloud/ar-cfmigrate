using System.Collections.Generic;

namespace CfMigrate.Cloudflare.Models
{
    public class BaseCloudflareError
    {
        public bool Success { get; set; }
        public List<string> Messages { get; set; }
        public List<string> Errors { get; set; }
    }
}