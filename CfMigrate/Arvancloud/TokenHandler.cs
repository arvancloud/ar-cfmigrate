namespace CfMigrate.Arvancloud
{
    public class ArvanTokenHandler
    {
        private static string _token;
        private static readonly object Lock = new();
        private static ArvanTokenHandler _tokenHandler;

        private ArvanTokenHandler(string token)
        {
            _token = token;
        }

        public static ArvanTokenHandler Instance(string token)
        {
            if (_tokenHandler is null)
            {
                lock (Lock)
                {
                    _tokenHandler ??= new ArvanTokenHandler(token);
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