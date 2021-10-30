using System.Collections.Generic;

namespace CloudFlareLib.Models
{
    public class BaseCloudflareError
    {
        public bool Success { get; set; }
        public List<string> Messages { get; set; }
        public List<string> Errors { get; set; }
    }
}