namespace HiriseLib.Tree
{
    internal interface ITreeConnection
    {
        void Store(Folder folder, IClientSession clientSession);
        void Store(Item item, string data, IClientSession clientSession);
    }
}
