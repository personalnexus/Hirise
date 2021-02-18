using System.Threading.Tasks;

namespace HiriseLib.Tree
{
    internal interface ITreeConnection
    {
        // Folders

        ValueTask LoadAsync(Folder folder);
        ValueTask StoreAsync(Folder folder, IClientSession clientSession);

        // Items

        ValueTask LoadAsync(Item item);
        ValueTask StoreAsync(Item item, IClientSession clientSession);

        // Access Control

        IAccessControl AccessControl { get; }
    }
}
