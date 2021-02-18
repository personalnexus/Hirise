using HiriseLib.Tree;
using ShUtilities.IO;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace HiriseLib.Redis
{
    internal class TreeUpdator: IDisposable
    {
        public TreeUpdator(Tree.Tree tree, StackExchange.Redis.ISubscriber subscriber)
        {
            _thisSender = Guid.NewGuid().ToString();
            _tree = tree;
            _subscriber = subscriber;
            _channelMessageSerializer = new JsonSerializer<TreeUpdateChannelMessageData>();
            subscriber.SubscribeAsync(DatabaseSchema.TreeUpdateChannel).ContinueWith(t => t.Result.OnMessage(ProcessItemUpdate), TaskContinuationOptions.LongRunning);
        }

        public void Dispose()
        {
            _subscriber.UnsubscribeAsync(DatabaseSchema.TreeUpdateChannel);
        }

        private readonly string _thisSender;
        private readonly Tree.Tree _tree;
        private readonly StackExchange.Redis.ISubscriber _subscriber;
        private readonly ISerializer<TreeUpdateChannelMessageData> _channelMessageSerializer; //TODO: replace with something faster than Json?

        private void ProcessItemUpdate(ChannelMessage message)
        {
            TreeUpdateChannelMessageData messageData = _channelMessageSerializer.DeserializeString(message.Message);
            if (messageData.Sender != _thisSender)
            {
                Item item = _tree.GetOrAddItem(messageData.Path);
                item.DataAsString = messageData.Data;
                item.NotifySubscribers();
            }
        }

        internal void PublishItemUpdate(Item item)
        {
            var messageData = new TreeUpdateChannelMessageData(_thisSender, item);
            string messageDataString = _channelMessageSerializer.SerializeString(messageData);
            _subscriber.PublishAsync(DatabaseSchema.TreeUpdateChannel, messageDataString);
        }
    }
}
