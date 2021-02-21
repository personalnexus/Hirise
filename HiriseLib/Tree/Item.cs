using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShUtilities.Collections;

namespace HiriseLib.Tree
{
    internal class Item : TreeElement, IItem
    {
        public Item(string name, Folder parentFolder, Tree tree) : base(name, parentFolder, tree)
        {
            _subscribersWithAccess = new HashSet<ISubscriber>();
            _subscribersWithoutAccess = new HashSet<ISubscriber>();
            tree.ItemCount++;
        }

        public override string Path => Protocol.CombineFolderAndItem(_parentFolder.Path, Name);

        public async ValueTask LoadAsync() => await _tree.Connection.LoadAsync(this);

        public async ValueTask StoreAsync(IClientSession clientSession) => await _tree.Connection.StoreAsync(this, clientSession);

        public string DataAsString { get; set; }

        public IList<IFolder> DataAsFolderList
        {
            get => DataAsString.Split(Protocol.NewLine).SelectWhere<string, IFolder>(_tree.TryGetFolder).ToList();
            set => DataAsString = Protocol.ElementsToStringList(value);
        }

        public IList<IItem> DataAsItemList
        {
            get => DataAsString.Split(Protocol.NewLine).SelectWhere<string, IItem>(_tree.TryGetItem).ToList();
            set => DataAsString = Protocol.ElementsToStringList(value);
        }

        // Subscriptions

        private readonly HashSet<ISubscriber> _subscribersWithAccess;
        private readonly HashSet<ISubscriber> _subscribersWithoutAccess;

        public async ValueTask<bool> AddSubscriberAsync(ISubscriber subscriber)
        {
            bool hasAccess = await _tree.Connection.AccessControl.HasReadAccessAsync(subscriber.Name, this);
            (hasAccess ? _subscribersWithAccess : _subscribersWithoutAccess).Add(subscriber);
            subscriber.ProcessSubscriptionBeginning(this, hasAccess);
            if (DataAsString != null)
            {
                subscriber.ProcessSubscriptionUpdate(this);
            }
            return hasAccess;
        }

        public void NotifySubscribers()
        {
            foreach (ISubscriber subscriber in _subscribersWithAccess)
            {
                subscriber.ProcessSubscriptionUpdate(this);
            }
        }

        public bool RemoveSubscriber(ISubscriber subscriber)
        {
            bool result = _subscribersWithAccess.Remove(subscriber) || _subscribersWithoutAccess.Remove(subscriber);
            if (result)
            {
                subscriber.ProcessSubscriptionEnding(this);
            }
            return result;
        }
    }
}
