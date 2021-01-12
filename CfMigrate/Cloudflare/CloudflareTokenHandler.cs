namespace CfMigrate.Cloudflare
{
    public class CloudflareTokenHandler
    {
        private static string _token;
        private static readonly object Lock = new();
        private static CloudflareTokenHandler _tokenHandler;

        private CloudflareTokenHandler(string token)
        {
            _token = token;
        }

        public static CloudflareTokenHandler Instance(string token)
        {
            if (_tokenHandler is null)
            {
                lock (Lock)
                {
                    _tokenHandler ??= new CloudflareTokenHandler(token);
                }
            }

            return _tokenHandler;
        }

        public static string GetToken()
        {
            return _token;
        }
    }
}