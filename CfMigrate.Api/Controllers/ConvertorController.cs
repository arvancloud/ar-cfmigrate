using System.Threading.Tasks;
using ArvancloudLib.Api.Token;
using CloudFlareLib.Api.Token;
using CloudFlareLib.Api.Zone;
using Microsoft.AspNetCore.Mvc;

namespace CfMigrate.Api.Controllers
{
    public class ConvertorController : BaseApi
    {
        private readonly ICloudflareZoneService _cloudflareZoneService;
        private readonly ICloudflareTokenService _cloudflareTokenService;

        private readonly IArvancloudTokenService _arvancloudTokenService;

        public ConvertorController(ICloudflareTokenService cloudflareTokenService,
            IArvancloudTokenService arvancloudTokenService, ICloudflareZoneService cloudflareZoneService)
        {
            _cloudflareTokenService = cloudflareTokenService;
            _arvancloudTokenService = arvancloudTokenService;
            _cloudflareZoneService = cloudflareZoneService;
        }

        [HttpGet]
        public async Task<bool> ConvertFromCloudflareToArvanCloud(string arvanToken, string cloudflareToken)
        {
            _arvancloudTokenService.SetLocalToken(arvanToken);
            _cloudflareTokenService.SetLocalToken(cloudflareToken);

            var domains = await _cloudflareZoneService.GetDomainsFromZone();

            return true;
        }
    }
}