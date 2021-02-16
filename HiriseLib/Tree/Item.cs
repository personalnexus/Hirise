using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShUtilities.Collections;

namespace HiriseLib.Tree
{
    internal class Item: TreeElement, IItem
    {
        public Item(string name, Folder parentFolder, Tree tree): base(name, parentFolder, tree)
        {
        }

        public override string Path => Protocol.CombineFolderAndItem(_parentFolder.Path, Name);

        public async ValueTask LoadAsync() => await _tree.Connection.LoadAsync(this);

        public async ValueTask StoreAsync(IClientSession clientSession) => await _tree.Connection.StoreAsync(this, clientSession);

        public string DataAsString { get; set; }
        public IList<IFolder> DataAsFolderList
        {
            get => DataAsString.Split(Protocol.NewLine).SelectWhere<string, IFolder>(_tree.TryGetFolder).ToList();
            set => DataAsString = Protocol.ElementsToStringList(value);
        }

        public IList<IItem> DataAsItemList
        {
            get => DataAsString.Split(Protocol.NewLine).SelectWhere<string, IItem>(_tree.TryGetItem).ToList();
            set => DataAsString = Protocol.ElementsToStringList(value);
        }
    }
}
