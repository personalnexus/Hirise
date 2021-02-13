using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var loadTasks = new List<Task>();

            var result = new Tree.Tree(this);
            foreach (IServer server in _connectionMultiplexer.GetEndPoints().Select(serverEndPoint => _connectionMultiplexer.GetServer(serverEndPoint)))
            {
                foreach (RedisKey key in server.Keys(pattern: DatabaseSchema.TreePattern))
                {
                    Protocol.SplitFolderAndItems(key, out string[] folders, out string itemName);
                    ValueTask loadValueTask = string.IsNullOrEmpty(itemName) ? LoadAsync(result.GetOrAddFolder(folders)) : LoadAsync(result.GetOrAddItem(folders, itemName));
                    loadTasks.Add(loadValueTask.AsTask());
                }
            }
            Task.WaitAll(loadTasks.ToArray());

            return result;
        }

        // Folders

        private async ValueTask LoadAsync(Folder folder)
        {
            await LoadElementAsync(folder);
        }

        public async ValueTask StoreAsync(Folder folder, IClientSession clientSession)
        {
            PrepareTreeStoreElement(clientSession, 0, out HashEntry[] hashEntries, out ElementStoreInfo lastStoreInfo);
            await StoreTreeElementAsync(folder, hashEntries, lastStoreInfo);
        }

        // Items

        private async ValueTask LoadAsync(Item item)
        {
            HashEntry[] hashEntries = await LoadElementAsync(item);
            item.Data = hashEntries.GetString("Data");
        }

        public async ValueTask StoreAsync(Item item, string data, IClientSession clientSession)
        {
            PrepareTreeStoreElement(clientSession, 1, out HashEntry[] hashEntries, out ElementStoreInfo lastStoreInfo);
            hashEntries[0] = new HashEntry("Data", data);
            await StoreTreeElementAsync(item, hashEntries, lastStoreInfo);
            item.Data = data;
        }

        // Utilities

        private async ValueTask<HashEntry[]> LoadElementAsync(TreeElement element)
        {
            HashEntry[] result = await _database.HashGetAllAsync(element.Path);
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

        private async ValueTask StoreTreeElementAsync(TreeElement element, HashEntry[] hashEntries, ElementStoreInfo lastStoreInfo)
        {
            await _database.HashSetAsync(Protocol.TreeNamespace + element.Path, hashEntries);
            element.LastStoreInfo = lastStoreInfo;
        }
    }
}
