using Microsoft.AspNetCore.Mvc;
using DotNetGigs.ViewModels;
using AutoMapper;
using DotNetGigs.Models.Entities;
using System.Threading.Tasks;
using DotNetGigs.Data;


namespace DotNetGigs.Controllers
{
    [Route("api/[controller]")]
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly IMapper _mapper;

        public ChatController(IMapper mapper, ApplicationDbContext appDbContext)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
        }

        // POST api/chat/createroom
        [HttpPost("createroom")]
        public async Task<IActionResult> Post([FromBody]RoomViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var room = _mapper.Map<Room>(model);
            // Room room = new Room { Title = model.Title };


            await _appDbContext.Rooms.AddAsync(room);
            await _appDbContext.SaveChangesAsync();

            return Ok(new
            {
                id = room.Id,
                title = room.Title
            });
            // return new OkObjectResult("Room created");
        }

    }
}