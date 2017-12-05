using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DotNetGigs
{
    [Authorize(Policy = "User")]
    public class Chat : Hub
    {
        private readonly static ConnectionMapping<string> _connections =
            new ConnectionMapping<string>();

        public override Task OnConnectedAsync()
        {
            string name = Context.User.Identity.Name;

            _connections.Add(name, Context.ConnectionId);
            Clients.All.InvokeAsync("UpdateUsers", _connections.Keys);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            string name = Context.User.Identity.Name;

            _connections.Remove(name, Context.ConnectionId);
            Clients.All.InvokeAsync("UpdateUsers", _connections.Keys);

            return base.OnDisconnectedAsync(exception);
        }

        public Task AddUser(string userName)
        {
            _connections.Add(userName, Context.ConnectionId);
            return Clients.All.InvokeAsync("UpdateUsers", _connections.Keys);
        }

        public Task RemoveUser(string userName)
        {
            _connections.Remove(userName, Context.ConnectionId);
            return Clients.All.InvokeAsync("UpdateUsers", _connections.Keys);
        }

        public Task Send(string message, int roomId)
        {
            return Clients.All.InvokeAsync("Send", message);
        }
    }
}