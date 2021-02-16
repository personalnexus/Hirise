namespace HiriseLib
{
    public interface IPathElement
    {
        string Name { get; }
        string Path { get; }
        IFolder ParentFolder { get; }
    }
}
