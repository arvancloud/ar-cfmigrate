using System.Threading.Tasks;

namespace ArvanCloudLib.Api.Domain
{
    public interface IArvancloudDomainService
    {
        Task<string> CreateNewDomain(string domain);
    }
}