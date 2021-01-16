using System.Threading.Tasks;

namespace CfMigrate.Api.Service
{
    public interface IConvertor
    {
        Task<bool> ConvertFromCloudflareToArvanCloud();
    }
}