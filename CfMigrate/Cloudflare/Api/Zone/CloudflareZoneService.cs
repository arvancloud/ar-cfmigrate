using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CfMigrate.Cloudflare.Handle;
using CfMigrate.Cloudflare.Models;
using CfMigrate.Cloudflare.Models.Zone;
using CfMigrate.Setting;
using Flurl.Http;
using Polly;
using Polly.Retry;

namespace CfMigrate.Cloudflare.Api.Zone
{
    public class CloudflareZoneService : ICloudflareZoneService
    {
        private readonly string _token;
        private readonly string _baseApi;
        private const string UrlParam = "zones";
        private readonly AsyncRetryPolicy _polly;

        public CloudflareZoneService()
        {
            _token = CloudflareTokenHandler.GetCompleteToken();
            _baseApi = UrlHandle.ConcatBaseUrlWithParam(BaseCloudflareValue.BaseApi, UrlParam);

            _polly = Policy
                .Handle<FlurlHttpTimeoutException>()
                .WaitAndRetryAsync(PollySetting.RetrySetting);
        }

        public async Task<List<DomainWithZoneIdentifierOutput>> GetDomainsFromZone()
        {
            try
            {
                var result = await _polly
                    .ExecuteAsync(async () =>
                    await _baseApi
                        .WithHeader(BaseCloudflareValue.Authorization, _token)
                        .GetAsync()
                        .ReceiveJson<BaseCloudflareModel<List<ZoneOutput>>>()
                );

                //todo we can add automapper
                var domains = result.Result
                    .Select(a => new DomainWithZoneIdentifierOutput
                    {
                        Id = a.Id,
                        Name = a.Name
                    }).ToList();

                return domains;
            }
            catch (FlurlHttpTimeoutException)
            {
                throw new Exception("Timeout after some retry");
            }
            catch (FlurlHttpException e)
            {
                throw new Exception(await ErrorHandle.ErrorHandler(e));
            }
        }
    }
}