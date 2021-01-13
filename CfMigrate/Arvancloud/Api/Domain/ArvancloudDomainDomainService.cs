using System;
using System.Threading.Tasks;
using CfMigrate.Arvancloud.Models;
using CfMigrate.Arvancloud.Models.Domain;
using CfMigrate.Setting;
using Flurl;
using Flurl.Http;
using Polly;
using Polly.Retry;

namespace CfMigrate.Arvancloud.Api.Domain
{
    public class ArvancloudDomainDomainService : IArvancloudDomainService
    {
        private readonly string _token;
        private readonly string _baseApi;
        private const string UrlParam = "domains";
        private readonly AsyncRetryPolicy _polly;

        public ArvancloudDomainDomainService()
        {
            _token = ArvanTokenHandler.GetToken();
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
                        .WithOAuthBearerToken(_token)
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