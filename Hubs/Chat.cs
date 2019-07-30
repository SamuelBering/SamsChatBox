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
using Microsoft.EntityFrameworkCore;
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

            var connections = _roomConnections.GetConnections(roomViewModel);
            var usernames = connections.Select(c => c.Name).Distinct();

            var pastMessages = _appDbContext.Messages.Include(m => m.Sender)
                                .Where(m => m.DateTime.Date == DateTime.Now.Date && m.RoomId == roomViewModel.Id).ToList();
            var messagesViewModel = pastMessages.Select(m => new MessageViewModel
            {
                Id = m.Id,
                Content = m.Content,
                Sender = m.Sender.UserName,
                DateTime = m.DateTime
            });

            // SendSeveralToRoom(messagesViewModel, roomViewModel);
            SendSeveralToUser(messagesViewModel, connection.ConnectionId);
            // InvokeAsyncInRoom(roomViewModel, "send", message, Context.User.Identity.Name);

            // return Clients.All.InvokeAsync("UpdateUsers", _connections.Keys);
            return InvokeAsyncInRoom(roomViewModel, "UpdateUsers", usernames);
        }

        private DateTime SaveMessage(string message, RoomViewModel room)
        {
            var claimsIdentity = Context.User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var userPkClaim = claimsIdentity.Claims
                    .SingleOrDefault(c => c.Type == Helpers.Constants.Strings.JwtClaimIdentifiers.Id);

                if (userPkClaim != null)
                {
                    var userIdValue = userPkClaim.Value;
                    var currentTime = DateTime.Now;

                    Message messageDb = new Message
                    {
                        Content = message,
                        SenderId = userIdValue,
                        RoomId = room.Id,
                        DateTime = currentTime
                    };

                    _appDbContext.Messages.Add(messageDb);
                    _appDbContext.SaveChanges();

                    return currentTime;
                }

            }

            throw new InvalidOperationException("Can´t find user \"Id\" claim");

        }

        private Task InvokeAsyncInRoom(RoomViewModel room, string method, params object[] args)
        {
            var connections = _roomConnections.GetConnections(room).ToList();
            foreach (var connection in connections)
                Clients.Client(connection.ConnectionId).InvokeAsync(method, args);

            return Task.CompletedTask;
        }


        public Task SendToRoom(string message, RoomViewModel roomViewModel)
        {
            DateTime currentTime = SaveMessage(message, roomViewModel);
            return InvokeAsyncInRoom(roomViewModel, "send", message, Context.User.Identity.Name, currentTime);
        }

        public Task SendSeveralToUser(IEnumerable<MessageViewModel> messages, string connectionId)
        {
            foreach (MessageViewModel mess in messages)
            {
                var sender = mess.Sender;
                var message = mess.Content;
                var dateTime = mess.DateTime;

                Clients.Client(connectionId).InvokeAsync("send", message, sender, dateTime);

                // InvokeAsyncInRoom(roomViewModel, "send", message, sender, dateTime);
            }


            return Task.CompletedTask;
        }
        public Task SendSeveralToRoom(IEnumerable<MessageViewModel> messages, RoomViewModel roomViewModel)
        {
            foreach (MessageViewModel mess in messages)
            {
                var sender = mess.Sender;
                var message = mess.Content;
                var dateTime = mess.DateTime;
                InvokeAsyncInRoom(roomViewModel, "send", message, sender, dateTime);
            }

            return Task.CompletedTask;

            // var connections = _roomConnections.GetConnections(roomViewModel).ToList();
            // foreach (var connection in connections)
            //     Clients.Client(connection.ConnectionId).InvokeAsync("send", message, Context.User.Identity.Name);
            // return InvokeAsyncInRoom(roomViewModel, "send", message, Context.User.Identity.Name);


        }

    }
}