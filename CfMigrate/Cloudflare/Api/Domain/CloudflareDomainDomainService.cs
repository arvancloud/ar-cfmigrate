using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CfMigrate.Cloudflare.Models;
using CfMigrate.Cloudflare.Models.Token;
using CfMigrate.Cloudflare.Models.Zone;
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
        private const string TokenType = "Bearer";
        private const string Authorization = "Authorization";
        private readonly AsyncRetryPolicy _polly;

        public CloudflareDomainDomainService()
        {
            _baseApi = BaseCloudflareValue.BaseApi + UrlParam;
            _token = TokenType + " " + CloudflareTokenHandler.GetToken();

            _polly = Policy
                .Handle<FlurlHttpTimeoutException>()
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2)
                });
        }

        public async Task<List<DomainWithZoneIdentifierOutput>> GetDomainsFromZone()
        {
            try
            {
                var result = await _polly.ExecuteAsync(async () =>
                    await _baseApi
                        .WithHeader(Authorization, _token)
                        .GetAsync()
                        .ReceiveJson<BaseCloudflareModel<List<ZoneOutput>>>()
                );

                //todo we can add automapper
                var domains = result.Result
                    .Select(a => new DomainWithZoneIdentifierOutput
                    {
                        Id = a.Id,
                        Name = a.Name
                    }).ToList();

                return domains;
            }
            catch (FlurlHttpTimeoutException)
            {
                throw new Exception("Timeout after some retry");
            }
            catch (FlurlHttpException e)
            {
                throw new Exception(await ErrorHandler(e));
            }
        }

        public async Task<bool> VerifyToken()
        {
            try
            {
                var result = await _polly.ExecuteAsync(async () =>
                    await _baseApi
                        .WithHeader(Authorization, _token)
                        .GetAsync()
                        .ReceiveJson<BaseCloudflareModel<VerifyTokenOutput>>()
                );

                return result.Result.Status.ToLower().Equals("active");
            }
            catch (FlurlHttpTimeoutException)
            {
                throw new Exception("Timeout after some retry");
            }
            catch (FlurlHttpException e)
            {
                throw new Exception(await ErrorHandler(e));
            }
        }

        private static async Task<string> ErrorHandler(FlurlHttpException e)
        {
            try
            {
                var error = await e.GetResponseJsonAsync<BaseCloudflareError>();

                return string.Join(",", error.Messages);
            }
            catch (Exception)
            {
                return "Unknown error";
            }
        }
    }
}