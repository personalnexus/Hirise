using StackExchange.Redis;

namespace HiriseLib.Redis
{
    internal static class DatabaseSchema
    {
        internal const string ChannelNamespace = "C:" + Protocol.FolderSeparatorString;
        internal const string UserNamespace = "U:" + Protocol.FolderSeparatorString;
        internal const string TreeNamespace = "T:" + Protocol.FolderSeparatorString;


        internal static readonly RedisChannel TreeUpdateChannel = new RedisChannel(ChannelNamespace + Protocol.CombineFolders("System", "TreeUpdate"), RedisChannel.PatternMode.Literal);

        internal static readonly RedisValue TreePattern = new RedisValue("T:*");
    }
}
