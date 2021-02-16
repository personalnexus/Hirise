namespace HiriseLib.Tree
{
    internal class Tree: ITree
    {
        public Tree(ITreeConnection connection)
        {
            Connection = connection;
            _root = new Folder("", null, this);
        }

        private readonly Folder _root;
        public ITreeConnection Connection { get; }

        // Internally used methods return internal classes

        public Folder GetOrAddFolder(string[] folders)
        {
            Folder result = _root;
            foreach (string folder in folders)
            {
                result = result.GetOrAddSubFolder(folder);
            }
            return result;
        }

        public Item GetOrAddItem(string[] folders, string itemName) => GetOrAddFolder(folders).GetOrAddSubItem(itemName);

        // ITree: implementations return the external interfaces

        public IFolder GetOrAddFolder(string folderPath) => GetOrAddFolder(Protocol.SplitFolders(folderPath));

        IFolder ITree.GetOrAddFolder(string[] folders) => GetOrAddFolder(folders);

        internal bool TryGetFolder(string folderPath, out IFolder resultFolder) => TryGetFolder(Protocol.SplitFolders(folderPath), out resultFolder);

        public bool TryGetFolder(string[] folders, out IFolder folder)
        {
            bool result = true;
            Folder resultFolder = _root;
            foreach (string folderName in folders)
            {
                if (!resultFolder.TryGetSubFolder(folderName, out resultFolder))
                {
                    result = false;
                    break;
                }
            }
            folder = resultFolder;
            return result;
        }

        public IItem GetOrAddItem(string itemPath)
        {
            Protocol.SplitFolderAndItems(itemPath, out string[] folders, out string itemName);
            return GetOrAddItem(folders, itemName);
        }

        IItem ITree.GetOrAddItem(string[] folders, string itemName) => GetOrAddItem(folders, itemName);

        internal bool TryGetItem(string itemPath, out IItem output)
        {
            output = null;
            Protocol.SplitFolderAndItems(itemPath, out string[] folders, out string itemName);
            bool result = TryGetFolder(folders, out IFolder folder) && folder.TryGetSubItem(itemName, out output);
            return result;
        }
    }
}
