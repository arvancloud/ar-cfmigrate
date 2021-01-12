namespace CfMigrate.Cloudflare
{
    public class CloudflareService : ICloudflareService
    {
        private readonly string _token;
        private readonly string _baseApi;
        private const string ApiVersion = "v4";

        public CloudflareService(string token)
        {
            _token = token;

            _baseApi = "https://api.cloudflare.com/client/" + ApiVersion + "/";
        }
    }
}