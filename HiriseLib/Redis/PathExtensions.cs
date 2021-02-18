using StackExchange.Redis;

namespace HiriseLib.Redis
{
    internal static class PathExtensions

    {
        internal static string PathInTreeNamespace(this IPathElement element) => DatabaseSchema.TreeNamespace + element.Path;
        internal static string WithoutTreeNamespace(this RedisKey key) => ((string)key)[DatabaseSchema.TreeNamespace.Length..];
    }
}
