using System.Threading.Tasks;

namespace HiriseLib
{
    public interface IFolder: IPathElement
    {
        ElementStoreInfo LastStoreInfo { get; }
        ValueTask StoreAsync(IClientSession clientSession);
        bool TryGetSubFolder(string folderName, out IFolder folder);
        bool TryGetSubItem(string itemName, out IItem item);
    }
}