using System;

namespace HiriseLib.Clients
{
    internal class Client : IClient
    {
        public Client(string name)
        {
            Name = name;
        }

        public Client(string name, string lastLoginEndpoint, DateTime lastLoginTimestamp): this(name)
        {
            LastLoginEndpoint = lastLoginEndpoint;
            LastLoginTimestamp = lastLoginTimestamp;
        }

        public string Name { get; }
        public string LastLoginEndpoint { get; set; }
        public DateTime? LastLoginTimestamp { get; set; }
    }
}
