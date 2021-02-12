using ShUtilities.Time;
using System;

namespace HiriseLib
{
    public readonly struct ElementStoreInfo
    {
        public ElementStoreInfo(IClientSession clientSession)
        {
            Timestamp = TimeProvider.Default.Now;
            ClientName = clientSession.Name;
            ClientEndPoint = clientSession.Endpoint;
        }

        public ElementStoreInfo(string clientName, string clientEndPoint, DateTime? timestamp)
        {
            Timestamp = timestamp;
            ClientName = clientName;
            ClientEndPoint = clientEndPoint;
        }

        public readonly DateTime? Timestamp;
        public readonly string ClientName;
        public readonly string ClientEndPoint;
    }
}
