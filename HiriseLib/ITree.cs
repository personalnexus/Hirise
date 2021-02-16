namespace HiriseLib
{
    public interface ITree
    {
        // Folders
        IFolder GetOrAddFolder(string folderPath);
        IFolder GetOrAddFolder(string[] folders);
        bool TryGetFolder(string[] folders, out IFolder folder);

        // Items
        IItem GetOrAddItem(string itemPath);
        IItem GetOrAddItem(string[] folders, string itemName);
    }
}
