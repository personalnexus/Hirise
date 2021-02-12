using System;

namespace HiriseLib
{
    public interface IClient
    {
        string Name { get; }
        string LastLoginEndpoint { get; }
        DateTime? LastLoginTimestamp { get; }
    }
}