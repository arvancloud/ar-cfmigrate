using System.Threading.Tasks;

namespace CfMigrate.Arvancloud.Api.Domain
{
    public interface IArvancloudDomainService
    {
        Task<string> CreateNewDomain(string domain);
    }
}