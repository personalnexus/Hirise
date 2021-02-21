using ShUtilities.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HiriseLib.Tree
{
    internal class Folder: TreeElement, IFolder
    {
        public Folder(string name, Folder parentFolder, Tree tree): base(name, parentFolder, tree)
        {
            _items = new Dictionary<string, Item>();
            _subFolders = new Dictionary<string, Folder>();
            if (tree != null)
            {
                tree.FolderCount++;
            }
        }

        // Folders

        private readonly Dictionary<string, Folder> _subFolders;

        public override string Path => _parentFolder == null ? Name : Protocol.CombineFolders(_parentFolder.Path, Name);

        internal Folder GetOrAddSubFolder(string name) => _subFolders.GetOrAdd(name, x => new Folder(name, this, _tree));

        public bool TryGetSubFolder(string folderName, out IFolder resultFolder)
        {
            bool result = TryGetSubFolder(folderName, out Folder folder);
            resultFolder = folder;
            return result;
        }

        internal bool TryGetSubFolder(string name, out Folder folder) => _subFolders.TryGetValue(name, out folder);

        // Items

        private readonly Dictionary<string, Item> _items;

        internal Item GetOrAddSubItem(string name) => _items.GetOrAdd(name, x => new Item(name, this, _tree));

        public bool TryGetSubItem(string itemName, out IItem resultItem)
        {
            bool result = _items.TryGetValue(itemName, out Item item);
            resultItem = item;
            return result;
        }

        // Load/Store

        public ValueTask StoreAsync(IClientSession clientSession) => _tree.Connection.StoreAsync(this, clientSession);
    }
}
