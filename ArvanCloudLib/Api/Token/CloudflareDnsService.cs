using ArvanCloudLib;

namespace ArvancloudLib.Api.Token
{
    public class ArvancloudTokenService : IArvancloudTokenService
    {
        public void SetLocalToken(string token)
        {
            ArvanTokenHandler.SetToken(token);
        }
    }
}