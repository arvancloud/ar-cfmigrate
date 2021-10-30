using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CfMigrate.Cloudflare.Handle;
using CfMigrate.Cloudflare.Models;
using CfMigrate.Cloudflare.Models.Dns;
using CfMigrate.Setting;
using Flurl;
using Flurl.Http;
using Polly;
using Polly.Retry;

namespace CfMigrate.Cloudflare.Api.Dns
{
    public class CloudflareDnsService : ICloudflareDnsService
    {
        private readonly string _token;
        private readonly string _baseApi;
        private const string UrlParam = "zones";
        private readonly AsyncRetryPolicy _polly;

        public CloudflareDnsService()
        {
            _token = CloudflareTokenHandler.GetCompleteToken();
            _baseApi = UrlHandle.ConcatBaseUrlWithParam(BaseCloudflareValue.BaseApi, UrlParam);

            _polly = Policy
                .Handle<FlurlHttpTimeoutException>()
                .WaitAndRetryAsync(PollySetting.RetrySetting);
        }

        public async Task<List<ShortDnsOutput>> GetDns(string zoneIdentifier)
        {
            try
            {
                var result = await _polly
                    .ExecuteAsync(async () =>
                        await _baseApi
                            .AppendPathSegment(zoneIdentifier + "/" + "dns_records")
                            .WithHeader(BaseCloudflareValue.Authorization, _token)
                            .GetAsync()
                            .ReceiveJson<BaseCloudflareModel<List<DnsOutput>>>()
                    );

                var items = result.Result
                    .Select(a => new ShortDnsOutput
                    {
                        Name = a.Name,
                        Ttl = a.Ttl,
                        Type = a.Type
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