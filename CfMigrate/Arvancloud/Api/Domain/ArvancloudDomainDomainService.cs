using System;
using System.Threading.Tasks;
using CfMigrate.Arvancloud.Models;
using CfMigrate.Arvancloud.Models.Domain;
using Flurl.Http;
using Polly;
using Polly.Retry;

namespace CfMigrate.Arvancloud.Api.Domain
{
    public class ArvancloudDomainDomainService : IArvancloudDomainService
    {
        private readonly string _token;
        private readonly string _baseApi;
        private const string UrlParam = "Domain";
        private readonly AsyncRetryPolicy _polly;

        public ArvancloudDomainDomainService()
        {
            _token = ArvanTokenHandler.GetToken();
            _baseApi = BaseArvanValue.BaseApi + UrlParam;

            _polly = Policy
                .Handle<FlurlHttpTimeoutException>()
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2)
                });
        }

        public async Task<bool> CreateNewDomain(string domain)
        {
            try
            {
                var result = await _polly.ExecuteAsync(async () =>
                    await _baseApi
                        .WithOAuthBearerToken(_token)
                        .PostJsonAsync(new DomainInput
                        {
                            Domain = domain
                        })
                        .ReceiveJson<BaseArvanModel<DomainOutput>>()
                );

                return true;
            }
            catch (FlurlHttpTimeoutException)
            {
                throw new Exception("Timeout");
            }
            catch (FlurlHttpException e)
            {
                var error = await e.GetResponseJsonAsync<BaseArvanError>();

                throw new Exception(error.Message);
            }
        }
    }
}