using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CfMigrate.Cloudflare.Handle;
using CfMigrate.Cloudflare.Models;
using CfMigrate.Cloudflare.Models.Dns;
using CfMigrate.Cloudflare.Models.DnsSec;
using CfMigrate.Setting;
using Flurl;
using Flurl.Http;
using Polly;
using Polly.Retry;

namespace CfMigrate.Cloudflare.Api.DnsSec
{
    public class CloudflareDnsSecService : ICloudflareDnsSecService
    {
        private readonly string _token;
        private readonly string _baseApi;
        private const string UrlParam = "zones";
        private readonly AsyncRetryPolicy _polly;

        public CloudflareDnsSecService()
        {
            _token = CloudflareTokenHandler.GetCompleteToken();
            _baseApi = UrlHandle.ConcatBaseUrlWithParam(BaseCloudflareValue.BaseApi, UrlParam);

            _polly = Policy
                .Handle<FlurlHttpTimeoutException>()
                .WaitAndRetryAsync(PollySetting.RetrySetting);
        }

        public async Task<List<ShortDnsSecOutput>> GetDnsSec(string zoneIdentifier)
        {
            try
            {
                var result = await _polly
                    .ExecuteAsync(async () =>
                        await _baseApi
                            .AppendPathSegment(zoneIdentifier + "/" + "dnssec")
                            .WithHeader(BaseCloudflareValue.Authorization, _token)
                            .GetAsync()
                            .ReceiveJson<BaseCloudflareModel<List<DnsSecOutput>>>()
                    );

                var items = result.Result
                    .Select(a => new ShortDnsSecOutput
                    {
                        Status = a.Status,
                        Ds = a.Ds
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