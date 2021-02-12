namespace HiriseLib
{
    public interface IFolder
    {
        string Name { get; }
        IFolder ParentFolder { get; }
        ElementStoreInfo LastStoreInfo { get; }
        void Store(IClientSession client);
    }
}