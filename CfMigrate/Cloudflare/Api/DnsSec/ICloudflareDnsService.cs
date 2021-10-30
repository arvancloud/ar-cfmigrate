using System.Collections.Generic;
using System.Threading.Tasks;
using CfMigrate.Cloudflare.Models.DnsSec;

namespace CfMigrate.Cloudflare.Api.DnsSec
{
    public interface ICloudflareDnsSecService
    {
        Task<List<ShortDnsSecOutput>> GetDnsSec(string zoneIdentifier);
    }
}