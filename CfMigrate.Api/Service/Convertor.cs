using System.Threading.Tasks;
using ArvanCloudLib.Api.Dns;
using ArvanCloudLib.Api.Domain;
using ArvanCloudLib.Models.Dns;
using CloudFlareLib.Api.Dns;
using CloudFlareLib.Api.Zone;

namespace CfMigrate.Api.Service
{
    public class Convertor : IConvertor

    {
        private readonly ICloudflareDnsService _cloudflareDnsService;
        private readonly ICloudflareZoneService _cloudflareZoneService;

        private readonly IArvancloudDnsService _arvancloudDnsService;
        private readonly IArvancloudDomainService _arvancloudDomainService;

        public Convertor(ICloudflareZoneService cloudflareZoneService, IArvancloudDomainService arvancloudDomainService,
            ICloudflareDnsService cloudflareDnsService, IArvancloudDnsService arvancloudDnsService)
        {
            _cloudflareZoneService = cloudflareZoneService;
            _arvancloudDomainService = arvancloudDomainService;
            _cloudflareDnsService = cloudflareDnsService;
            _arvancloudDnsService = arvancloudDnsService;
        }

        public async Task<bool> ConvertFromCloudflareToArvanCloud()
        {
            var domains = await _cloudflareZoneService.GetDomainsFromZone();

            foreach (var domain in domains)
            {
                await _arvancloudDomainService.CreateNewDomain(domain.Name);

                var flareDns = await _cloudflareDnsService.GetDns(domain.Id);

                foreach (var dnsOutput in flareDns)
                {
                    await _arvancloudDnsService.CreateDns(new DnsInput
                    {
                        Name = dnsOutput.Name,
                        Domain = domain.Name,
                        //Ttl = dnsOutput.Ttl,
                    });
                }
            }

            return true;
        }
    }
}