using System.Collections.Generic;
using System.Threading.Tasks;
using CloudFlareLib.Models.DnsSec;

namespace CloudFlareLib.Api.DnsSec
{
    public interface ICloudflareDnsSecService
    {
        Task<List<DnsSecOutput>> GetDnsSec(string zoneIdentifier);
    }
}