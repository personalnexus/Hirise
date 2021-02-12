namespace HiriseLib.Tree
{
    internal abstract class TreeElement
    {
        protected TreeElement(string name, Folder parentFolder, ITreeConnection connection)
        {
            _connection = connection;
            Name = name;
            _parentFolder = parentFolder;
        }

        protected readonly ITreeConnection _connection;
        protected readonly Folder _parentFolder;

        public string Name { get; }
        public abstract string Path { get; }
        public IFolder ParentFolder => _parentFolder;
        public ElementStoreInfo LastStoreInfo { get; set; }

    }
}
