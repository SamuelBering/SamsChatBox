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

        // public void SendChatMessage(string fromWho, string who, string message)
        // {
        //     string name = Context.User.Identity.Name;

        //     foreach (var connectionId in _connections.GetConnections(who))
        //     {
        //         Clients.Client(connectionId).addChatMessage(name + ": " + message);
        //     }
        // }

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

        public Task Send(string message)
        {
            return Clients.All.InvokeAsync("Send", message);
        }
    }
}