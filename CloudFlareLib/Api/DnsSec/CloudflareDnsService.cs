using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CloudFlareLib.Handle;
using CloudFlareLib.Models;
using CloudFlareLib.Models.DnsSec;
using CloudFlareLib.Setting;
using Flurl;
using Flurl.Http;
using Polly;
using Polly.Retry;

namespace CloudFlareLib.Api.DnsSec
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

        public async Task<List<DnsSecOutput>> GetDnsSec(string zoneIdentifier)
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

                return result.Result;
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