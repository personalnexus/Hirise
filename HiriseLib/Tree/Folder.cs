using ShUtilities.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HiriseLib.Tree
{
    internal class Folder: TreeElement, IFolder
    {
        public Folder(string name, Folder parentFolder, ITreeConnection connection): base(name, parentFolder, connection)
        {
            _items = new Dictionary<string, Item>();
            _subFolders = new Dictionary<string, Folder>();
        }

        private readonly Dictionary<string, Folder> _subFolders;
        private readonly Dictionary<string, Item> _items;

        public override string Path => _parentFolder == null ? Name : Protocol.CombineFolders(_parentFolder.Path, Name);

        internal Folder GetOrAddSubFolder(string name) => _subFolders.GetOrAdd(name, x => new Folder(name, this, _connection));

        internal Item GetOrAddSubItem(string name) => _items.GetOrAdd(name, x => new Item(name, this, _connection));

        public ValueTask StoreAsync(IClientSession clientSession) => _connection.StoreAsync(this, clientSession);
    }
}
