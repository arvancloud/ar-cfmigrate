namespace CfMigrate.Cloudflare
{
    public class CloudflareService : ICloudflareService
    {
        private readonly string _token;
        
        public CloudflareService(string token)
        {
            _token = token;
        }
    }
}