namespace CfMigrate.Cloudflare
{
    public static class BaseCloudflareValue
    {
        private const string ApiVersion = "v4";
        public static readonly string TokenType = "Bearer";
        public static readonly string Authorization = "Authorization";
        public static readonly string BaseApi = "https://api.cloudflare.com/client/" + ApiVersion + "/";
    }
}