using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private readonly static ConnectionMapping<RoomViewModel> _roomConnections =
        new ConnectionMapping<RoomViewModel>();

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
                ConnectionId = Context.ConnectionId,
                Name = name
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
            Connection connection = new Connection
            {
                ConnectionId = Context.ConnectionId,
                Name = name
            };

            var previousRoom = _roomConnections.FindKey(connection);

            if (previousRoom != null)
                _roomConnections.Remove(previousRoom, connection);

            _roomConnections.Add(roomViewModel, connection);

            return Clients.All.InvokeAsync("UpdateUsers", _connections.Keys);
        }

        private void SaveMessage(string message, RoomViewModel room)
        {
            //Get the UserId
            var claimsIdentity = Context.User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                // the principal identity is a claims identity.
                // now we need to find the NameIdentifier claim
                var userPkClaim = claimsIdentity.Claims
                    .SingleOrDefault(c => c.Type == Helpers.Constants.Strings.JwtClaimIdentifiers.Id);

                if (userPkClaim != null)
                {
                    var userIdValue = userPkClaim.Value;
                }
            }
            // Message mess = new Message
            // {
            //     Content = message,
            //     IdentityId = Context.Connection.User.Claims["id"]
            // };
        }
        public Task SendToRoom(string message, RoomViewModel roomViewModel)
        {
            SaveMessage(message, roomViewModel);

            var connections = _roomConnections.GetConnections(roomViewModel).ToList();
            foreach (var connection in connections)
                Clients.Client(connection.ConnectionId).InvokeAsync("send", message, Context.User.Identity.Name);

            return Task.CompletedTask;
        }

    }
}