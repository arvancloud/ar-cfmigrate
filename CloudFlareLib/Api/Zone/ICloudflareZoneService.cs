using System.Collections.Generic;
using System.Threading.Tasks;
using CloudFlareLib.Models.Zone;

namespace CloudFlareLib.Api.Zone
{
    public interface ICloudflareZoneService
    {
        Task<List<DomainWithZoneIdentifierOutput>> GetDomainsFromZone();
    }
}