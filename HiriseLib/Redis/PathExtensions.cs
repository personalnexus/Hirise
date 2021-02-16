using StackExchange.Redis;

namespace HiriseLib.Redis
{
    internal static class PathExtensions

    {
        internal static string PathInTreeNamespace(this IPathElement element) => Protocol.TreeNamespace + element.Path;
        internal static string WithoutTreeNamespace(this RedisKey key) => ((string)key)[Protocol.TreeNamespace.Length..];
    }
}
