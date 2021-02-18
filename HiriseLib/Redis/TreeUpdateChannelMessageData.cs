using HiriseLib.Tree;

namespace HiriseLib.Redis
{
    internal class TreeUpdateChannelMessageData
    {
        public TreeUpdateChannelMessageData()
        {
        }

        public TreeUpdateChannelMessageData(string sender, Item item)
        {
            //TODO: add other meta data
            Sender = sender;
            Path = item.Path;
            Data = item.DataAsString;
        }

        public string Sender { get; set; }
        public string Path { get; set; }
        public string Data { get; set; }
    }
}
