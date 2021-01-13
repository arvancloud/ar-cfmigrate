using System.Collections.Generic;
using System.Threading.Tasks;
using CloudFlareLib.Models.Dns;

namespace CloudFlareLib.Api.Dns
{
    public interface ICloudflareDnsService
    {
        Task<List<DnsOutput>> GetDns(string zoneIdentifier);
    }
}