namespace HiriseLib.Tree
{
    internal class Tree: ITree
    {
        public Tree(ITreeConnection connection)
        {
            _root = new Folder("", null, connection);
        }

        private readonly Folder _root;

        internal Item GetOrAddItem(string[] folderPath, string itemName) => GetOrAddFolder(folderPath).GetOrAddSubItem(itemName);

        internal Folder GetOrAddFolder(string[] folders)
        {
            Folder result = _root;
            foreach (string folder in folders)
            {
                result = result.GetOrAddSubFolder(folder);
            }
            return result;
        }

        // ITree

        public IFolder GetOrAdd(string[] folders) => GetOrAddFolder(folders);

        public IItem GetOrAdd(string[] folders, string itemName) => GetOrAddItem(folders, itemName);
    }
}
