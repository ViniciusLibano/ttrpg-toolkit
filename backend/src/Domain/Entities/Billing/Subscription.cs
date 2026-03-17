using TTRPG.Toolkit.Domain.Enums;
using TTRPG.Toolkit.Domain.Shared;

namespace TTRPG.Toolkit.Domain.Entities.Billing;

public class Subscription
{
    public Guid Id { get; private set; }
    public Guid OwnerId { get; private set; }

    public string PlanName { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsActive => DateTime.UtcNow <= ExpiresAt;

    private readonly List<SubscriptionMember> _members = [];
    public IReadOnlyCollection<SubscriptionMember> Members => _members.AsReadOnly();

    public Subscription(Guid ownerId, string planName, DateTime expiresAt)
    {
        Id = Guid.CreateVersion7();
        OwnerId = ownerId;
        PlanName = planName;
        ExpiresAt = expiresAt;
    }

    public Result AddFamilyMember(Guid userId)
    {
        if (!IsActive)
            return Result.Failure(
                "Subscription.Inactive",
                "A assinatura está inativa ou expirada.",
                ErrorType.Validation);

        if (_members.Count >= 5)
            return Result.Failure(
                "Subscription.MemberLimitReached",
                "O limite de 5 convites para o plano família foi atingido.",
                ErrorType.Validation);

        if (_members.Any(m => m.UserId == userId))
            return Result.Failure(
                "Subscription.UserAlreadyAdded",
                "Este usuário já faz parte desta assinatura.",
                ErrorType.Conflict);

        _members.Add(new SubscriptionMember(this.Id, userId));
        return Result.Success();
    }

    public Result RemoveFamilyMember(Guid userId)
    {
        if (_members.FirstOrDefault(m => m.UserId == userId) is not {} member) 
            return Result.Failure(
                "Subscription.UserNotAdded",
                "Este usuário não faz parte dessa assinatura.",
                ErrorType.Conflict);

        _members.Remove(member);
        return Result.Success();
    }
}