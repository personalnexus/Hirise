using StackExchange.Redis;

namespace HiriseLib.Redis
{
    internal static class DatabaseSchema
    {
        internal static readonly RedisValue TreePattern = new RedisValue(Protocol.TreeNamespace + "*");
    }
}
