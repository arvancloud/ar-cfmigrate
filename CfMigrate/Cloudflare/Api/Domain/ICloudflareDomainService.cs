using System.Collections.Generic;
using System.Threading.Tasks;
using CfMigrate.Cloudflare.Models.Zone;

namespace CfMigrate.Cloudflare.Api.Domain
{
    public interface ICloudflareDomainService
    {
        Task<bool> VerifyToken();
        Task<List<DomainWithZoneIdentifierOutput>> GetDomainsFromZone();
    }
}