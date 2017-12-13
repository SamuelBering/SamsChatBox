using System.Collections.Generic;
using System.Linq;
using DotNetGigs.ViewModels;

namespace DotNetGigs
{
    public class Connection
    {
        public string ConnectionId { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            Connection connection = obj as Connection;
            return connection.ConnectionId.Equals(this.ConnectionId) && connection.Name.Equals(this.Name);
        }

        public override int GetHashCode()
        {
            return ConnectionId.GetHashCode() ^ Name.GetHashCode();
        }

    }
}