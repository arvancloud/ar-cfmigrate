using System;
using System.Threading.Tasks;
using ArvanCloudLib.Models;
using ArvanCloudLib.Models.Domain;
using ArvanCloudLib.Setting;
using Flurl;
using Flurl.Http;
using Polly;
using Polly.Retry;

namespace ArvanCloudLib.Api.Domain
{
    public class ArvancloudDomainService : IArvancloudDomainService
    {
        private readonly string _token;
        private readonly string _baseApi;
        private const string UrlParam = "domains";
        private readonly AsyncRetryPolicy _polly;

        public ArvancloudDomainService()
        {
            _token = ArvanTokenHandler.GetCompleteToken();
            _baseApi = BaseArvanValue.BaseApi + UrlParam;

            _polly = Policy
                .Handle<FlurlHttpTimeoutException>()
                .WaitAndRetryAsync(PollySetting.RetrySetting);
        }

        public async Task<string> CreateNewDomain(string domain)
        {
            try
            {
                var result = await _polly.ExecuteAsync(async () =>
                    await _baseApi
                        .AppendPathSegment("dns-service")
                        .WithHeader(BaseArvanValue.Authorization, _token)
                        .PostJsonAsync(new DomainInput
                        {
                            Domain = domain
                        })
                        .ReceiveJson<BaseArvanModel<DomainOutput>>()
                );

                return result.Data.Domain;
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