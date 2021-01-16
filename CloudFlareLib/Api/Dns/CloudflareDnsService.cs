using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CloudFlareLib.Handle;
using CloudFlareLib.Models;
using CloudFlareLib.Models.Dns;
using CloudFlareLib.Setting;
using Flurl;
using Flurl.Http;
using Polly;
using Polly.Retry;

namespace CloudFlareLib.Api.Dns
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

        public async Task<List<DnsOutput>> GetDns(string zoneIdentifier)
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