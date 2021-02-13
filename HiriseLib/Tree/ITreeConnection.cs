using System.Threading.Tasks;

namespace HiriseLib.Tree
{
    internal interface ITreeConnection
    {
        ValueTask StoreAsync(Folder folder, IClientSession clientSession);
        ValueTask StoreAsync(Item item, string data, IClientSession clientSession);
    }
}
