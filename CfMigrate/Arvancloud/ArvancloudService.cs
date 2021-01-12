namespace CfMigrate.Arvancloud
{
    public class ArvancloudService : IArvancloudService
    {
        private readonly string _token;
        
        public ArvancloudService(string token)
        {
            _token = token;
        }
    }
}