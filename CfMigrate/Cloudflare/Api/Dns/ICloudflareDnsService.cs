using System.Collections.Generic;
using System.Threading.Tasks;
using CfMigrate.Cloudflare.Models.Dns;

namespace CfMigrate.Cloudflare.Api.Dns
{
    public interface ICloudflareDnsService
    {
        Task<List<ShortDnsOutput>> GetDns(string zoneIdentifier);
    }
}