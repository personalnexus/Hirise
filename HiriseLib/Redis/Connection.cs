using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiriseLib.AccessControl;
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
            _accessControl = new UnrestrictedAccess();
    }

        public void Dispose()
        {
            _connectionMultiplexer?.Dispose();
            _connectionMultiplexer = null;
            _database = null;
        }

        // Access Control

        public IAccessControl AccessControl => _accessControl;

        private readonly UnrestrictedAccess _accessControl;

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
            HashEntry[] hashEntries = _database.HashGetAll(DatabaseSchema.UserNamespace + client.Name);
            client.LastLoginEndpoint = hashEntries.GetString("LastLoginEndPoint");
            client.LastLoginTimestamp = hashEntries.GetTimestamp("LastLoginTimestamp");
        }

        private void Store(IClientSession clientSession)
        {
            var hashEntries = new HashEntry[2];
            hashEntries.Set(0, "LastLoginTimestamp", TimeProvider.Default.Now);
            hashEntries.Set(1, "LastLoginEndPoint", clientSession.Endpoint);
            _database.HashSet(DatabaseSchema.UserNamespace + clientSession.Name, hashEntries);
        }

        // Tree

        private TreeUpdator _treeUpdator;

        public ITree InitializeTree()
        {
            if (_treeUpdator != null)
            {
                throw new InvalidOperationException("Tree already initialized");
            }

            var tree = new Tree.Tree(this);
            _treeUpdator = new TreeUpdator(tree, _connectionMultiplexer.GetSubscriber());

            var loadTasks = new List<Task>();
            foreach (IServer server in _connectionMultiplexer.GetEndPoints().Select(serverEndPoint => _connectionMultiplexer.GetServer(serverEndPoint)))
            {
                foreach (RedisKey key in server.Keys(pattern: DatabaseSchema.TreePattern))
                {
                    Protocol.SplitFolderAndItems(key.WithoutTreeNamespace(), out string[] folders, out string itemName);
                    ValueTask loadValueTask = string.IsNullOrEmpty(itemName) ? LoadAsync(tree.GetOrAddFolder(folders)) : LoadAsync(tree.GetOrAddItem(folders, itemName));
                    loadTasks.Add(loadValueTask.AsTask());
                }
            }
            Task.WaitAll(loadTasks.ToArray());

            return tree;
        }

        // Folders

        public async ValueTask LoadAsync(Folder folder)
        {
            await LoadElementAsync(folder);
        }

        public async ValueTask StoreAsync(Folder folder, IClientSession clientSession)
        {
            PrepareTreeStoreElement(clientSession, 0, out HashEntry[] hashEntries, out ElementStoreInfo lastStoreInfo);
            await StoreTreeElementAsync(folder, hashEntries, lastStoreInfo);
        }

        // Items

        public async ValueTask LoadAsync(Item item)
        {
            HashEntry[] hashEntries = await LoadElementAsync(item);
            item.DataAsString = hashEntries.GetString("Data");
        }

        public async ValueTask StoreAsync(Item item, IClientSession clientSession)
        {
            PrepareTreeStoreElement(clientSession, 1, out HashEntry[] hashEntries, out ElementStoreInfo lastStoreInfo);
            hashEntries.Set(0, "Data", item.DataAsString);
            _treeUpdator.PublishItemUpdate(item);
            await StoreTreeElementAsync(item, hashEntries, lastStoreInfo);
        }

        // Utilities

        private async ValueTask<HashEntry[]> LoadElementAsync(TreeElement element)
        {
            HashEntry[] result = await _database.HashGetAllAsync(element.PathInTreeNamespace());
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
            await _database.HashSetAsync(element.PathInTreeNamespace(), hashEntries);
            element.LastStoreInfo = lastStoreInfo;
        }
    }
}
