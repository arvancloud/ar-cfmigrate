using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudFlareLib.Handle;
using CloudFlareLib.Models;
using CloudFlareLib.Models.Zone;
using CloudFlareLib.Setting;
using Flurl.Http;
using Polly;
using Polly.Retry;

namespace CloudFlareLib.Api.Zone
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

                var items = result.Result
                    .Select(a => new DomainWithZoneIdentifierOutput
                    {
                        Id = a.Id,
                        Name = a.Name
                    }).ToList();

                return items;
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