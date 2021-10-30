using System;
using System.Threading.Tasks;
using ArvanCloudLib.Models;
using ArvanCloudLib.Models.Dns;
using ArvanCloudLib.Setting;
using Flurl;
using Flurl.Http;
using Polly;
using Polly.Retry;

namespace ArvanCloudLib.Api.Dns
{
    public class ArvancloudDnsService : IArvancloudDnsService
    {
        private readonly string _token;
        private readonly string _baseApi;
        private const string UrlParam = "domains";
        private readonly AsyncRetryPolicy _polly;

        public ArvancloudDnsService()
        {
            _token = ArvanTokenHandler.GetCompleteToken();
            _baseApi = BaseArvanValue.BaseApi + UrlParam;

            _polly = Policy
                .Handle<FlurlHttpTimeoutException>()
                .WaitAndRetryAsync(PollySetting.RetrySetting);
        }

        public async Task<string> CreateDns(DnsInput dns)
        {
            try
            {
                var result = await _polly.ExecuteAsync(async () =>
                    await _baseApi
                        .AppendPathSegment("dns-records")
                        .WithHeader(BaseArvanValue.Authorization, _token)
                        .PostJsonAsync(dns)
                        .ReceiveJson<BaseArvanModel<DnsOutput>>()
                );

                return result.Data.Name;
            }
            catch (FlurlHttpTimeoutException)
            {
                throw new Exception("Timeout after some retry");
            }
            catch (FlurlHttpException e)
            {
                var error = await e.GetResponseJsonAsync<BaseArvanError>();

                throw new Exception(error.Message);
            }
        }
    }
}