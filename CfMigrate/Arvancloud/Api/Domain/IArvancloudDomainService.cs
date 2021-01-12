using System.Threading.Tasks;

namespace CfMigrate.Arvancloud.Api.Domain
{
    public interface IArvancloudDomainService
    {
        Task<bool> CreateNewDomain(string domain);
    }
}