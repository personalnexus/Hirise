namespace HiriseLib
{
    public interface IItem
    {
        string Name { get; }
        IFolder ParentFolder { get; }
        ElementStoreInfo LastStoreInfo { get; }
        void Store(string data, IClientSession clientSession);

        string Data { get; }
    }
}