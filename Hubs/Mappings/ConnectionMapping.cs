using System.Collections.Generic;
using System.Linq;

namespace DotNetGigs

{
    public class ConnectionMapping<T>
    {
        private readonly Dictionary<T, HashSet<Connection>> _connections =
            new Dictionary<T, HashSet<Connection>>();

        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }

        public IEnumerable<T> Keys
        {
            get
            {
                return this._connections.Keys;
            }
        }

        public void Add(T key, Connection connection)
        {
            lock (_connections)
            {
                HashSet<Connection> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<Connection>();
                    _connections.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connection);
                }
            }
        }

        public IEnumerable<Connection> GetConnections(T key)
        {
            HashSet<Connection> connections;
            if (_connections.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<Connection>();
        }

        public void Remove(T key, Connection connection)
        {
            lock (_connections)
            {
                HashSet<Connection> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    return;
                }

                lock (connections)
                {
                    connections.Remove(connection);

                    if (connections.Count == 0)
                    {
                        _connections.Remove(key);
                    }
                }
            }
        }
    }
}