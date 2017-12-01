using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;

namespace DotNetGigs.Auth
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);
        ClaimsIdentity GenerateClaimsIdentity(string userName, string id);
        IEnumerable<Claim> GenerateClaims(string id, bool apiAcess = false);
    }
}