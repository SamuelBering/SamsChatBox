using Microsoft.AspNetCore.Mvc;
using DotNetGigs.ViewModels;
using AutoMapper;
using DotNetGigs.Models.Entities;
using System.Threading.Tasks;
using DotNetGigs.Data;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using Newtonsoft.Json;

namespace DotNetGigs.Controllers
{
    [Route("api/[controller]")]
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IHubContext<Chat> _chat;
         private readonly JsonSerializerSettings _serializerSettings;

        public ChatController(IHubContext<Chat> chat, IMapper mapper, ApplicationDbContext appDbContext)
        {
            _chat = chat;
            _mapper = mapper;
            _appDbContext = appDbContext;
             _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
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

            await _appDbContext.Rooms.AddAsync(room);
            await _appDbContext.SaveChangesAsync();

            var allRooms = _appDbContext.Rooms.Select(r => new
            {
                id = r.Id,
                title = r.Title
            }).ToList();

            var allRoomsJson = JsonConvert.SerializeObject(allRooms, _serializerSettings);

            await _chat.Clients.All.InvokeAsync("UpdateRoomsList", allRoomsJson);

            return Ok(new
            {
                id = room.Id,
                title = room.Title
            });
        }

    }
}