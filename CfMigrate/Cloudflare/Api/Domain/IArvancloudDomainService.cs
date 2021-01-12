using System.Collections.Generic;
using System.Threading.Tasks;

namespace CfMigrate.Cloudflare.Api.Domain
{
    public interface ICloudflareDomainService
    {
        Task<List<string>> ListDomains();
    }
}