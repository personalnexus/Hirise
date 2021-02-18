using System.Threading.Tasks;

namespace HiriseLib
{
    public interface IAsyncSubscribable
    {
        ValueTask<bool> AddSubscriberAsync(ISubscriber subscriber);
        bool RemoveSubscriber(ISubscriber subscriber);
    }
}
