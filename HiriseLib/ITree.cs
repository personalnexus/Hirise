namespace HiriseLib
{
    public interface ITree
    {
        IFolder GetOrAdd(string[] folders);
        IItem GetOrAdd(string[] folders, string itemName);
    }
}
