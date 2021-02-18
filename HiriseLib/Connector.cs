using HiriseLib.AccessControl;
using HiriseLib.Clients;
using HiriseLib.Redis;
using ShUtilities.Common;
using System;

namespace HiriseLib
{
    public class Connector: IDisposable
    {
        public Connector(): this(new Connection())
        {
        }

        internal Connector(Connection connection)
        {
            _connection = connection;
            _lazyTree = new Lazy<ITree>(_connection.InitializeTree, false);
        }
        public void Dispose()
        {
            Disposer.Null(ref _connection);
        }

        private Connection _connection;
        private Lazy<ITree> _lazyTree;

        public ITree Tree => _lazyTree.Value;

        public IClientSession Login(string clientName, string clientEndpoint) => new ClientSession(clientName, clientEndpoint, _connection);

        public IClient GetClient(string clientName) => _connection.GetClient(clientName);
    }
}
