namespace CfMigrate.Cloudflare
{
    public static class BaseCloudflareValue
    {
        private const string ApiVersion = "v4";
        public static string BaseApi = "https://api.cloudflare.com/client/" + ApiVersion + "/";
    }
}