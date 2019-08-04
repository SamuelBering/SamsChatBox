using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace DotNetGigs.Services
{
    public interface IHttpClientService
    {
        Task<T> Get<T>(string url, DataContractJsonSerializerSettings settings = null);
    }
}