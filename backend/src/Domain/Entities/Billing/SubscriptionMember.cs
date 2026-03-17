namespace TTRPG.Toolkit.Domain.Entities.Billing;

public class SubscriptionMember
{
    public Guid UserId { get; private set; }
    public Guid SubscriptionId { get; private set; }
    public DateTime JoinedAt { get; private set; }
    
    protected SubscriptionMember()
    {}

    public SubscriptionMember(Guid userId, Guid subscriptionId)
    {
        UserId = userId;
        SubscriptionId = subscriptionId;
        JoinedAt = DateTime.UtcNow;
    }
}