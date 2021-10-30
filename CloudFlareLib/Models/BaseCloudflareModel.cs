using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CloudFlareLib.Models
{
    public class BaseCloudflareModel<T>
    {
        public bool Success { get; set; }
        public List<string> Messages { get; set; }
        public List<string> Errors { get; set; }
        
        [JsonPropertyName("result_info")]
        public PageInfo PageInfo { get; set; }
        public T Result { get; set; }
    }

    public class PageInfo
    {
        public int Page { get; set; }
        
        [JsonPropertyName("per_page")]
        public int PerPage { get; set; }
        
        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }
        
        public int Count { get; set; }
        
        [JsonPropertyName("total_count")]
        public int TotalCount { get; set; }
    }
}