using System.Collections.Generic;
using System.Threading.Tasks;

namespace HiriseLib
{
    public interface IItem: IPathElement, IAsyncSubscribable
    {
        ElementStoreInfo LastStoreInfo { get; }

        ValueTask LoadAsync();
        ValueTask StoreAsync(IClientSession clientSession);

        string DataAsString { get; set; }
        IList<IFolder> DataAsFolderList { get; set; }
        IList<IItem> DataAsItemList { get; set; }
    }
}