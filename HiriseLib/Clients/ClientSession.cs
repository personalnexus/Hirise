using System;
using System.Collections.Generic;

namespace HiriseLib.Clients
{
    internal class ClientSession : IClientSession, IDisposable
    {
        public ClientSession(string clientName, string clientEndpoint, IClientsConnection connection)
        {
            _subscribedItems = new HashSet<IAsyncSubscribable>();
            Name = clientName;
            Endpoint = clientEndpoint;
            IsLoggedIn = connection.Login(this);
        }

        public void Dispose()
        {
            IsLoggedIn = false;
            RemoveAllSubscriptions();
        }

        public string Name { get; }
        public string Endpoint { get; }
        public bool IsLoggedIn { get; private set; }

        // Subscriptions

        private HashSet<IAsyncSubscribable> _subscribedItems;

        public void ProcessSubscriptionBeginning(IItem item, bool hasAccess) => _subscribedItems.Add(item);

        public void ProcessSubscriptionUpdate(IItem item)
        {
            // TODO: do something useful
        }

        public void ProcessSubscriptionEnding(IItem item) => _subscribedItems?.Remove(item);

        private void RemoveAllSubscriptions()
        {
            // No need tp keep track of each subscription's end, when we're removing them all
            HashSet<IAsyncSubscribable> subscribedItemsCopy = _subscribedItems;
            _subscribedItems = null;
            foreach (IAsyncSubscribable item in subscribedItemsCopy)
            {
                item.RemoveSubscriber(this);
            }
        }
    }
}
