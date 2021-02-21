namespace HiriseLib
{
    public interface ITree
    {
        // Folders
        int FolderCount { get; }
        IFolder GetOrAddFolder(string folderPath);
        IFolder GetOrAddFolder(string[] folders);
        bool TryGetFolder(string[] folders, out IFolder folder);

        // Items
        int ItemCount { get; }
        IItem GetOrAddItem(string itemPath);
        IItem GetOrAddItem(string[] folders, string itemName);
    }
}
