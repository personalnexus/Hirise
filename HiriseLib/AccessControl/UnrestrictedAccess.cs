using System.Threading.Tasks;

namespace HiriseLib.AccessControl
{
    public class UnrestrictedAccess : IAccessControl
    {
        public ValueTask<bool> HasReadAccessAsync(string accessId, IPathElement item) => ValueTask.FromResult(true);
    }
}
