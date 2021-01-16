namespace ArvanCloudLib
{
    public static class BaseArvanValue
    {
        private const string ApiVersion = "4.0";
        public const string TokenType = "Bearer";
        public const string Authorization = "Authorization";
        public const string BaseApi = "https://napi.arvancloud.com/cdn/" + ApiVersion + "/";
    }
}