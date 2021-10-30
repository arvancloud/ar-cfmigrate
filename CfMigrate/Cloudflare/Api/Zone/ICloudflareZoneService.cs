using System.Collections.Generic;
using System.Threading.Tasks;
using CfMigrate.Cloudflare.Models.Zone;

namespace CfMigrate.Cloudflare.Api.Zone
{
    public interface ICloudflareZoneService
    {
        Task<List<DomainWithZoneIdentifierOutput>> GetDomainsFromZone();
    }
}