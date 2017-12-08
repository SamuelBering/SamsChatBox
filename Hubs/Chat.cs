using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetGigs.Data;
using DotNetGigs.Models.Entities;
using DotNetGigs.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace DotNetGigs
{
    [Authorize(Policy = "User")]
    public class Chat : Hub
    {
        private readonly static ConnectionMapping<string> _connections =
            new ConnectionMapping<string>();
        private readonly ApplicationDbContext _appDbContext;
        private readonly JsonSerializerSettings _serializerSettings;

        public Chat(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }
        public override Task OnConnectedAsync()
        {
            string name = Context.User.Identity.Name;

            Connection connection = new Connection
            {
                RoomViewModel = new RoomViewModel
                {
                    Id = 0,
                    Title = "(No room)"
                },
                ConnectionId = Context.ConnectionId
            };

            _connections.Add(name, connection);

            var allRooms = _appDbContext.Rooms.Select(r => new
            {
                id = r.Id,
                title = r.Title
            }).ToList();

            var allRoomsJson = JsonConvert.SerializeObject(allRooms, _serializerSettings);

            Clients.Client(Context.ConnectionId).InvokeAsync("UpdateRoomsList", allRoomsJson);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            string name = Context.User.Identity.Name;
            var connections = _connections.GetConnections(name);
            var currentConnection = connections.Single(c => c.ConnectionId == Context.ConnectionId);
            _connections.Remove(name, currentConnection);
            Clients.All.InvokeAsync("UpdateUsers", _connections.Keys);

            return base.OnDisconnectedAsync(exception);
        }

        public Task EnterRoom(RoomViewModel roomViewModel)
        {
            string name = Context.User.Identity.Name;
            var connections = _connections.GetConnections(name);
            var currentConnection = connections.Single(c => c.ConnectionId == Context.ConnectionId);
            currentConnection.RoomViewModel = roomViewModel;
            return Clients.All.InvokeAsync("UpdateUsers", _connections.Keys);
        }

        public Task Send(string message, int roomId)
        {
            return Clients.All.InvokeAsync("Send", message);
        }

    }
}