namespace CfMigrate.Arvancloud
{
    public class ArvancloudService : IArvancloudService
    {
        private readonly string _token;
        private readonly string _baseApi;
        private const string ApiVersion = "4.0";

        public ArvancloudService(string token)
        {
            _token = token;
            _baseApi = "https://napi.arvancloud.com/cdn/" + ApiVersion + "/";
        }
    }
}