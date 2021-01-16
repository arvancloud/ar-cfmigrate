namespace CloudFlareLib
{
    public static class BaseCloudflareValue
    {
        private const string ApiVersion = "v4";
        public const string TokenType = "Bearer";
        public const string Authorization = "Authorization";
        public const string BaseApi = "https://api.cloudflare.com/client/" + ApiVersion + "/";
    }
}