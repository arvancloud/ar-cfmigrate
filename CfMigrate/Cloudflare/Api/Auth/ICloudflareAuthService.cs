using System.Threading.Tasks;

namespace CfMigrate.Cloudflare.Api.Auth
{
    public interface ICloudflareAuthService
    {
        Task<bool> VerifyToken();
    }
}