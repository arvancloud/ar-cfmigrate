using System.Threading.Tasks;
using ArvancloudLib.Api.Token;
using CfMigrate.Api.Service;
using CloudFlareLib.Api.Token;
using Microsoft.AspNetCore.Mvc;

namespace CfMigrate.Api.Controllers
{
    public class ConvertorController : BaseApi
    {
        private readonly Convertor _convertor;
        private readonly ICloudflareTokenService _cloudflareTokenService;

        private readonly IArvancloudTokenService _arvancloudTokenService;

        public ConvertorController(ICloudflareTokenService cloudflareTokenService,
            IArvancloudTokenService arvancloudTokenService, Convertor convertor)
        {
            _convertor = convertor;
            _cloudflareTokenService = cloudflareTokenService;
            _arvancloudTokenService = arvancloudTokenService;
        }

        [HttpGet]
        public async Task<bool> ConvertFromCloudflareToArvanCloud(string arvanToken, string cloudflareToken)
        {
            _arvancloudTokenService.SetLocalToken(arvanToken);
            _cloudflareTokenService.SetLocalToken(cloudflareToken);

            var result = await _convertor.ConvertFromCloudflareToArvanCloud();

            return result;
        }
    }
}