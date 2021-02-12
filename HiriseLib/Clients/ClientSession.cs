using System;

namespace HiriseLib.Clients
{
    internal class ClientSession : IClientSession, IDisposable
    {
        public ClientSession(string clientName, string clientEndpoint, IClientsConnection connection)
        {
            Name = clientName;
            Endpoint = clientEndpoint;
            IsLoggedIn = connection.Login(this);
        }

        public void Dispose() => IsLoggedIn = false;

        public string Name { get; }
        public string Endpoint { get; }
        public bool IsLoggedIn { get; private set; }
    }
}
