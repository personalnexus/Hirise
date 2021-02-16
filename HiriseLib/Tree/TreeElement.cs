namespace HiriseLib.Tree
{
    internal abstract class TreeElement: IPathElement
    {
        protected TreeElement(string name, Folder parentFolder, Tree tree)
        {
            _tree = tree;
            Name = name;
            _parentFolder = parentFolder;
        }

        protected readonly Tree _tree;
        protected readonly Folder _parentFolder;

        public string Name { get; }
        public abstract string Path { get; }
        public IFolder ParentFolder => _parentFolder;
        public ElementStoreInfo LastStoreInfo { get; set; }

    }
}
