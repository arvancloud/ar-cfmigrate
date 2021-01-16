using System.Collections.Generic;

namespace CfMigrate.Arvancloud.Models
{
    public class BaseArvanError
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
    }
}