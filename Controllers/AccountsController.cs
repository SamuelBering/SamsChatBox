using Microsoft.AspNetCore.Mvc;
using DotNetGigs.ViewModels;
using AutoMapper;
using DotNetGigs.Models.Entities;
using Microsoft.AspNetCore.Identity;
using DotNetGigs.Helpers;
using System.Threading.Tasks;
using DotNetGigs.Data;
using DotNetGigs.Auth;

namespace DotNetGigs.Controllers
{
    [Route("api/[controller]")]
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IJwtFactory _jwtFactory;

        public AccountsController(UserManager<AppUser> userManager, IMapper mapper, ApplicationDbContext appDbContext, IJwtFactory jwtFactory)
        {
            _userManager = userManager;
            _mapper = mapper;
            _appDbContext = appDbContext;
            _jwtFactory = jwtFactory;
        }

        // POST api/accounts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentity = _mapper.Map<AppUser>(model);

            var result = await _userManager.CreateAsync(userIdentity, model.Password);
            // await _userManager.AddToRoleAsync(userIdentity, Helpers.Constants.Strings.JwtClaimIdentifiers.Rol);

            // var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            // result = await _userManager.AddClaimsAsync(userIdentity, _jwtFactory.GenerateClaimsIdentity(userIdentity.UserName, userIdentity.Id).Claims);

            result = await _userManager.AddClaimsAsync(userIdentity, _jwtFactory.GenerateClaims(userIdentity.Id));
            
            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            await _appDbContext.Jobseekers.AddAsync(new Jobseeker { IdentityId = userIdentity.Id, Location = model.Location });
            await _appDbContext.SaveChangesAsync();

            return new OkObjectResult("Account created");
        }


    }
}