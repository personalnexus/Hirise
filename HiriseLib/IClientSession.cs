using System;

namespace HiriseLib
{
    public interface IClientSession: IDisposable
    {
        string Name { get; }
        string Endpoint { get; }
        bool IsLoggedIn { get; }
    }
}
