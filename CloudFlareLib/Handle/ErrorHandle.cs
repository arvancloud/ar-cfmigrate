using System;
using System.Threading.Tasks;
using CloudFlareLib.Models;
using Flurl.Http;

namespace CloudFlareLib.Handle
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