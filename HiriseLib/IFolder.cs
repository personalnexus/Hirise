using System.Threading.Tasks;

namespace HiriseLib
{
    public interface IFolder
    {
        string Name { get; }
        IFolder ParentFolder { get; }
        ElementStoreInfo LastStoreInfo { get; }
        ValueTask StoreAsync(IClientSession clientSession);
    }
}