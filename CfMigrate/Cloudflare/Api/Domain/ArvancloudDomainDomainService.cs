using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CfMigrate.Arvancloud.Models.Domain;
using CfMigrate.Cloudflare.Models;
using Flurl.Http;
using Polly;
using Polly.Retry;

namespace CfMigrate.Cloudflare.Api.Domain
{
    public class CloudflareDomainDomainService : ICloudflareDomainService
    {
        private readonly string _token;
        private readonly string _baseApi;
        private const string UrlParam = "domains";
        private readonly AsyncRetryPolicy _polly;

        public CloudflareDomainDomainService()
        {
            _token = CloudflareTokenHandler.GetToken();
            _baseApi = BaseCloudflareValue.BaseApi + UrlParam;

            _polly = Policy
                .Handle<FlurlHttpTimeoutException>()
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2)
                });
        }

        public async Task<List<string>> ListDomains()
        {
            try
            {
                var result = await _polly.ExecuteAsync(async () =>
                    await _baseApi
                        .WithOAuthBearerToken(_token)
                        .PostJsonAsync(new DomainInput
                        {
                            Domain = ""
                        })
                        .ReceiveJson<BaseCloudflareModel<List<string>>>()
                );

                return result.Result;
            }
            catch (FlurlHttpTimeoutException)
            {
                throw new Exception("Timeout after some retry");
            }
            catch (FlurlHttpException e)
            {
                var error = await e.GetResponseJsonAsync<BaseCloudflareError>();

                throw new Exception(string.Join(",", error.Messages));
            }
        }
    }
}