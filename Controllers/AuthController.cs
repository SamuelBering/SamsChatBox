using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using DotNetGigs.Models.Entities;
using DotNetGigs.Models;
using DotNetGigs.Auth;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using DotNetGigs.Helpers;
using System.Security.Claims;
using System.Linq;
using System.Security.Principal;

namespace DotNetGigs.Controllers
{

    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly JwtIssuerOptions _jwtOptions;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;

            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody]CredentialsViewModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);
            if (identity == null)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
            }

            // Serialize and return the response
            var apiAccessClaim = identity.Claims.SingleOrDefault(c => c.Type == "rol" && c.Value == "api_access");

            var response = new
            {
                id = identity.Claims.Single(c => c.Type == "id").Value,
                hasApiAccess = apiAccessClaim != null ? true : false,
                auth_token = await _jwtFactory.GenerateEncodedToken(credentials.UserName, identity),
                expires_in = (int)_jwtOptions.ValidFor.TotalSeconds
            };

            var json = JsonConvert.SerializeObject(response, _serializerSettings);
            return new OkObjectResult(json);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                // get the user to verifty
                var userToVerify = await _userManager.FindByNameAsync(userName);

                if (userToVerify != null)
                {
                    // check the credentials  
                    if (await _userManager.CheckPasswordAsync(userToVerify, password))
                    {
                        var result = await _signInManager.CheckPasswordSignInAsync(userToVerify, password, false);

                        var claims = await _userManager.GetClaimsAsync(userToVerify);

                        _jwtFactory.AddUniqueNameClaim(claims, userName);
                        // var debugClaimsIdentidy = new ClaimsIdentity(new GenericIdentity(userName, "Token"), claims);
                        // var debugClaimsIdentidy = new ClaimsIdentity(claims);

                        // var claimsIdentity = _jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id);
                        var claimsIdentity = new ClaimsIdentity(new GenericIdentity(userName, "Token"), claims);

                        return await Task.FromResult(claimsIdentity);
                    }
                }
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }
    }
}