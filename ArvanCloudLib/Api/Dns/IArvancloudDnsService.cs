using System.Threading.Tasks;
using ArvanCloudLib.Models.Dns;

namespace ArvanCloudLib.Api.Dns
{
    public interface IArvancloudDnsService
    {
        Task<string> CreateDns(DnsInput dns);
    }
}