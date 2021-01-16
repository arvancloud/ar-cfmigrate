using System;
using System.Threading.Tasks;
using CloudFlareLib.Handle;
using CloudFlareLib.Models;
using CloudFlareLib.Models.Token;
using CloudFlareLib.Setting;
using Flurl;
using Flurl.Http;
using Polly;
using Polly.Retry;

namespace CloudFlareLib.Api.Auth
{
    public class CloudflareAuthService : ICloudflareAuthService
    {
        private readonly string _token;
        private readonly string _baseApi;
        private const string UrlParam = "user";
        private readonly AsyncRetryPolicy _polly;

        public CloudflareAuthService()
        {
            _token = CloudflareTokenHandler.GetCompleteToken();
            _baseApi = UrlHandle.ConcatBaseUrlWithParam(BaseCloudflareValue.BaseApi, UrlParam);

            _polly = Policy
                .Handle<FlurlHttpTimeoutException>()
                .WaitAndRetryAsync(PollySetting.RetrySetting);
        }

        public async Task<bool> VerifyToken()
        {
            try
            {
                var result = await _polly
                    .ExecuteAsync(async () =>
                    await _baseApi
                        .AppendPathSegment("tokens/verify")
                        .WithHeader(BaseCloudflareValue.Authorization, _token)
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
                throw new Exception(await ErrorHandle.ErrorHandler(e));
            }
        }
    }
}