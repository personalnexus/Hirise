using System.Threading.Tasks;

namespace HiriseLib
{
    public interface IItem
    {
        string Name { get; }
        IFolder ParentFolder { get; }
        ElementStoreInfo LastStoreInfo { get; }
        ValueTask StoreAsync(string data, IClientSession clientSession);

        string Data { get; }
    }
}