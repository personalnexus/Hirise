using System.Threading.Tasks;

namespace HiriseLib
{
    public interface IAccessControl
    {
        ValueTask<bool> HasReadAccessAsync(string accessId, IPathElement item);
    }
}
