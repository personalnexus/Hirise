using System;
using StackExchange.Redis;

namespace HiriseLib
{
    public class Connection: IDisposable
    {
        private ConnectionMultiplexer _connectionMultiplexer;
        private IDatabase _connection;
        private readonly RedisKey _userHash = new RedisKey($"Users:{Environment.UserName}");

        public Connection()
        {
            var options = new ConfigurationOptions
            {
                EndPoints = { "127.0.0.1:6379" },
                Password = "risinghigh",
                ResolveDns = true,
                //AbortOnConnectFail = false,
                KeepAlive = 180,
                ConnectTimeout = 10000,
            };
            _connectionMultiplexer = ConnectionMultiplexer.Connect(options);
            _connection = _connectionMultiplexer.GetDatabase();
            LastConnect = DateTime.UtcNow;

        }

        public void Dispose()
        {
            _connectionMultiplexer?.Dispose();
            _connectionMultiplexer = null;
            _connection = null;
        }

        public DateTime LastConnect 
        {
            get => DateTime.Parse(_connection.HashGet(_userHash, nameof(LastConnect)));
            set => _connection.HashSet(_userHash, nameof(LastConnect), value.ToString());
        }
    }
}
