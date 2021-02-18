using System;

namespace HiriseLib
{
    public interface IClientSession: IDisposable, ISubscriber
    {
        string Endpoint { get; }
        bool IsLoggedIn { get; }
    }
}
