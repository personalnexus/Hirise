namespace HiriseLib
{
    public interface ISubscriber
    {
        string Name { get; }
        void ProcessSubscriptionBeginning(IItem item, bool hasAccess);
        void ProcessSubscriptionUpdate(IItem item);
        void ProcessSubscriptionEnding(IItem item);
    }
}
