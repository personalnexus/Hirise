using StackExchange.Redis;

namespace HiriseLib.Redis
{
    public static class ChannelExtensions
    {
        public static string WithoutChannelNamespace(this RedisChannel channel) => ((string)channel)[DatabaseSchema.ChannelNamespace.Length..];
    }
}
