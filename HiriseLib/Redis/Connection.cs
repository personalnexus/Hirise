using System;
using System.Globalization;
using System.Linq;
using HiriseLib.Clients;
using HiriseLib.Tree;
using ShUtilities.Time;
using StackExchange.Redis;

namespace HiriseLib.Redis
{
    internal class Connection: IDisposable, ITreeConnection, IClientsConnection
    {
        private ConnectionMultiplexer _connectionMultiplexer;
        private IDatabase _database;

        public Connection()
        {
            var options = new ConfigurationOptions
            {
                EndPoints = { "127.0.0.1:6379" },
                Password = "risinghigh",
                ResolveDns = true,
                KeepAlive = 180,
                ConnectTimeout = 10000,
            };
            _connectionMultiplexer = ConnectionMultiplexer.Connect(options);
            _database = _connectionMultiplexer.GetDatabase();

        }

        public void Dispose()
        {
            _connectionMultiplexer?.Dispose();
            _connectionMultiplexer = null;
            _database = null;
        }

        // Clients

        public bool Login(IClientSession clientSession)
        {
            bool result;
            try
            {
                Store(clientSession);
                result = true;
            }
            catch (Exception)
            {
                result = false;
                //TODO: log exception
            }
            return result;
        }

        public Client GetClient(string clientName)
        {
            var result = new Client(clientName);
            Load(result);
            return result;
        }

        private void Load(Client client)
        {
            HashEntry[] hashEntries = _database.HashGetAll(Protocol.UserNamespace + client.Name);
            client.LastLoginEndpoint = hashEntries.GetString("LastLoginEndPoint");
            client.LastLoginTimestamp = hashEntries.GetTimestamp("LastLoginTimestamp");
        }

        private void Store(IClientSession clientSession)
        {
            var hashEntries = new HashEntry[2];
            hashEntries.Set(0, "LastLoginTimestamp", TimeProvider.Default.Now);
            hashEntries.Set(1, "LastLoginEndPoint", clientSession.Endpoint);
            _database.HashSet(Protocol.UserNamespace + clientSession.Name, hashEntries);
        }

        // Tree

        public ITree InitializeTree()
        {
            var result = new Tree.Tree(this);

            foreach (IServer server in _connectionMultiplexer.GetEndPoints().Select(serverEndPoint => _connectionMultiplexer.GetServer(serverEndPoint)))
            {
                foreach (RedisKey key in server.Keys(pattern: DatabaseSchema.TreePattern))
                {
                    Protocol.SplitFolderAndItems(key, out string[] folders, out string itemName);
                    if (string.IsNullOrEmpty(itemName))
                    {
                        Folder folder = result.GetOrAddFolder(folders);
                        Load(folder);
                    }
                    else
                    {
                        Item item = result.GetOrAddItem(folders, itemName);
                        Load(item);
                    }
                }
            }

            return result;
        }

        // Folders

        private void Load(Folder folder)
        {
            LoadElement(folder);
        }

        public void Store(Folder folder, IClientSession clientSession)
        {
            PrepareTreeStoreElement(clientSession, 0, out HashEntry[] hashEntries, out ElementStoreInfo lastStoreInfo);
            StoreTreeElement(folder, hashEntries, lastStoreInfo);
        }

        // Items

        private void Load(Item item)
        {
            HashEntry[] hashEntries = LoadElement(item);
            item.Data = hashEntries.GetString("Data");
        }

        public void Store(Item item, string data, IClientSession clientSession)
        {
            PrepareTreeStoreElement(clientSession, 1, out HashEntry[] hashEntries, out ElementStoreInfo lastStoreInfo);
            hashEntries[0] = new HashEntry("Data", data);
            StoreTreeElement(item, hashEntries, lastStoreInfo);
            item.Data = data;
        }

        // Utilities

        private HashEntry[] LoadElement(TreeElement element)
        {
            HashEntry[] result = _database.HashGetAll(element.Path);
            element.LastStoreInfo = GetLastStoreInfo(result);
            return result;
        }

        private ElementStoreInfo GetLastStoreInfo(HashEntry[] hashEntries)
        {
            DateTime? timestamp = hashEntries.GetTimestamp("LastStoreTimestamp");
            string clientName = hashEntries.GetString("LastStoreClientName");
            string clientEndpoint = hashEntries.GetString("LastStoreClientEndpoint");
            return new ElementStoreInfo(clientName, clientEndpoint, timestamp);
        }

        private void PrepareTreeStoreElement(IClientSession clientSession, int additionalEntryCount, out HashEntry[] hashEntries, out ElementStoreInfo lastStoreInfo)
        {
            lastStoreInfo = new ElementStoreInfo(clientSession);
            hashEntries = new HashEntry[3 + additionalEntryCount];
            hashEntries.Set(^1, "LastStoreTimestamp", lastStoreInfo.Timestamp);
            hashEntries.Set(^2, "LastStoreClientName", lastStoreInfo.ClientName);
            hashEntries.Set(^3, "LastStoreClientEndpoint", lastStoreInfo.ClientEndPoint);
        }

        private void StoreTreeElement(TreeElement element, HashEntry[] hashEntries, ElementStoreInfo lastStoreInfo)
        {
            _database.HashSet(Protocol.TreeNamespace + element.Path, hashEntries);
            element.LastStoreInfo = lastStoreInfo;
        }
    }
}
