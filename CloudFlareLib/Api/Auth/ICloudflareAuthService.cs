using System.Threading.Tasks;

namespace CloudFlareLib.Api.Auth
{
    public interface ICloudflareAuthService
    {
        Task<bool> VerifyToken();
    }
}