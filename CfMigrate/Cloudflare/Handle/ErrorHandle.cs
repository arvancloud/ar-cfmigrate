using System;
using System.Threading.Tasks;
using CfMigrate.Cloudflare.Models;
using Flurl.Http;

namespace CfMigrate.Cloudflare.Handle
{
    public static class ErrorHandle
    {
        public static async Task<string> ErrorHandler(FlurlHttpException e)
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