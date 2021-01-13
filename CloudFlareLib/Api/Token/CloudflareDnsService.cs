namespace CloudFlareLib.Api.Token
{
    public class CloudflareTokenService : ICloudflareTokenService
    {
        public void SetLocalToken(string token)
        {
            CloudflareTokenHandler.SetToken(token);
        }
    }
}