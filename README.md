# Tabletop RPG Toolkit - Development Plan

## Project Overview
A comprehensive SaaS platform for tabletop RPG enthusiasts, featuring:
- Character sheet designer
- Map maker
- Session manager
- Community features

## Technology Stack

### Backend
- **.NET Core 10** (ASP.NET Core)
- **Entity Framework Core 10** for ORM
- **PostgreSQL** for primary database
- **Redis** for caching and session management
- **Azure Storage** or **AWS S3** for file storage (maps, images, assets)

### Frontend
- **Next.js 15+** with React 19
- **TailwindCSS** for styling
- **Redux Toolkit** or **Zustand** for state management
- **SignalR** client for real-time features

### Infrastructure
- **Docker** for containerization
- **Kubernetes** for orchestration (if scaling needed)
- **GitHub Actions** or **Azure DevOps** for CI/CD

## Hosting Recommendations

### Development & MVP Phase
- **Backend:** Azure App Service or AWS Elastic Beanstalk
- **Database:** Azure Database for PostgreSQL or AWS RDS
- **Storage:** Azure Blob Storage or AWS S3
- **Frontend:** Vercel (optimal for Next.js) or Netlify
- **Cost:** ~$50-100/month for initial setup

### Production Phase
- **Option A: Azure Kubernetes Service (AKS)** - Best if using Azure ecosystem
- **Option B: Amazon EKS** - Best if using AWS ecosystem
- **Option C: DigitalOcean Kubernetes** - More cost-effective for bootstrapped startups
- **CDN:** CloudFront or Azure CDN for static assets
- **Monitoring:** Application Insights or DataDog

## Project Structure

```
/
├── backend/
│   ├── src/
│   │   ├── TTRPG Toolkit.API/                 # Web API project
│   │   ├── TTRPG Toolkit.Core/                 # Domain models, interfaces
│   │   ├── TTRPG Toolkit.Infrastructure/       # Data access, external services
│   │   ├── TTRPG Toolkit.Application/          # Business logic, services
│   │   ├── TTRPG Toolkit.Shared/               # DTOs, constants, extensions
│   │   └── TTRPG Toolkit.RealTime/             # SignalR hub for live sessions
│   ├── tests/
│   │   ├── TTRPG Toolkit.UnitTests/
│   │   ├── TTRPG Toolkit.IntegrationTests/
│   │   └── TTRPG Toolkit.FunctionalTests/
│   └── docker/
│       ├── Dockerfile.api
│       └── docker-compose.yml
│
├── frontend/
│   ├── app/                                    # Next.js app directory
│   │   ├── (auth)/
│   │   ├── (dashboard)/
│   │   ├── (community)/
│   │   ├── (tools)/
│   │   │   ├── sheet-designer/
│   │   │   ├── map-maker/
│   │   │   └── session-manager/
│   │   ├── api/                                # Next.js API routes (if needed)
│   │   └── layout.tsx
│   ├── components/
│   │   ├── ui/                                 # Reusable UI components
│   │   ├── forms/
│   │   ├── sheets/
│   │   └── maps/
│   ├── lib/
│   │   ├── api-client/                         # Auto-generated API client
│   │   ├── hooks/
│   │   └── utils/
│   └── public/
│
├── infrastructure/
│   ├── k8s/                                    # Kubernetes manifests
│   ├── terraform/                               # IaC for cloud resources
│   └── scripts/
│
├── docs/
│   ├── api/                                     # OpenAPI/Swagger docs
│   └── architecture/
│
├── .github/
│   └── workflows/
│       ├── ci.yml
│       └── cd.yml
│
├── README.md
└── .gitignore
```

## Identity & User Management Entities

### Core Identity Models

```csharp
// Backend/Core/Entities/Identity/User.cs
public class User : IdentityUser<Guid>
{
    public string DisplayName { get; set; }
    public string? AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public UserPreferences Preferences { get; set; }
    public SubscriptionTier SubscriptionTier { get; set; }
    
    // Navigation properties
    public virtual ICollection<UserGroup> UserGroups { get; set; }
    public virtual ICollection<GroupInvitation> SentInvitations { get; set; }
    public virtual ICollection<Campaign> OwnedCampaigns { get; set; }
    public virtual ICollection<CampaignMember> CampaignMemberships { get; set; }
}

// Backend/Core/Entities/Identity/Group.cs
public class Group
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid OwnerId { get; set; }
    
    // Navigation properties
    public virtual User Owner { get; set; }
    public virtual ICollection<UserGroup> Members { get; set; }
    public virtual ICollection<GroupInvitation> Invitations { get; set; }
    public virtual ICollection<Campaign> Campaigns { get; set; }
    public virtual Subscription Subscription { get; set; }
}

// Backend/Core/Entities/Identity/UserGroup.cs (Many-to-Many)
public class UserGroup
{
    public Guid UserId { get; set; }
    public Guid GroupId { get; set; }
    public GroupRole Role { get; set; } // Admin, Member, Viewer
    public DateTime JoinedAt { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; }
    public virtual Group Group { get; set; }
}

// Backend/Core/Entities/Identity/GroupInvitation.cs
public class GroupInvitation
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    public string InvitedEmail { get; set; }
    public Guid InvitedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public InvitationStatus Status { get; set; } // Pending, Accepted, Expired
    public string? InvitationToken { get; set; }
    
    // Navigation properties
    public virtual Group Group { get; set; }
    public virtual User Inviter { get; set; }
}
```

### Subscription Entities

```csharp
// Backend/Core/Entities/Billing/Subscription.cs
public class Subscription
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    public SubscriptionPlan Plan { get; set; } // Family (5-6 users), Premium, etc.
    public SubscriptionStatus Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? TrialEndsAt { get; set; }
    public int MaxMembers { get; set; } // 5 or 6 for family plan
    public string? StripeSubscriptionId { get; set; }
    public string? StripeCustomerId { get; set; }
    
    // Navigation properties
    public virtual Group Group { get; set; }
    public virtual ICollection<Payment> Payments { get; set; }
}

// Backend/Core/Entities/Billing/Payment.cs
public class Payment
{
    public Guid Id { get; set; }
    public Guid SubscriptionId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentStatus Status { get; set; }
    public string? StripePaymentIntentId { get; set; }
    public string? InvoiceUrl { get; set; }
    
    // Navigation properties
    public virtual Subscription Subscription { get; set; }
}
```

### Campaign Entities

```csharp
// Backend/Core/Entities/Game/Campaign.cs
public class Campaign
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public Guid GameSystemId { get; set; }
    public Guid GroupId { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public CampaignStatus Status { get; set; }
    
    // Navigation properties
    public virtual GameSystem GameSystem { get; set; }
    public virtual Group Group { get; set; }
    public virtual User Creator { get; set; }
    public virtual ICollection<CampaignMember> Members { get; set; }
    public virtual ICollection<Session> Sessions { get; set; }
    public virtual ICollection<Map> Maps { get; set; }
    public virtual ICollection<Character> Characters { get; set; }
}

// Backend/Core/Entities/Game/CampaignMember.cs
public class CampaignMember
{
    public Guid Id { get; set; }
    public Guid CampaignId { get; set; }
    public Guid UserId { get; set; }
    public CampaignRole Role { get; set; } // GameMaster, Player, Observer
    public DateTime JoinedAt { get; set; }
    
    // Navigation properties
    public virtual Campaign Campaign { get; set; }
    public virtual User User { get; set; }
}
```

## Database Schema Overview

```sql
-- Key tables structure
- Users (extends IdentityUser)
- Groups
- UserGroups (junction)
- GroupInvitations
- Subscriptions
- Payments
- Campaigns
- CampaignMembers
- Sessions
- Maps
- Characters
- CharacterSheets (JSON templates)
- Assets (images, tokens, etc.)
- Comments (community features)
- Likes
- Tags
```

## Development Phases

### Phase 1: Foundation (Weeks 1-4)
- [ ] Project setup with .NET 10 and Next.js
- [ ] Authentication system with ASP.NET Core Identity
- [ ] Group and invitation system
- [ ] Basic subscription management with Stripe integration
- [ ] PostgreSQL database setup with Entity Framework
- [ ] Docker development environment

### Phase 2: Core Features (Weeks 5-12)
- [ ] Character sheet designer (drag-and-drop builder)
- [ ] Sheet rendering engine
- [ ] Basic map maker (grid-based, token placement)
- [ ] Session scheduler and notes
- [ ] File upload for assets
- [ ] Real-time collaboration basics

### Phase 3: Community Features (Weeks 13-16)
- [ ] User profiles
- [ ] Content sharing (sheets, maps)
- [ ] Comments and ratings
- [ ] Search functionality
- [ ] Tagging system
- [ ] Community marketplace (optional)

### Phase 4: Polish & Launch (Weeks 17-20)
- [ ] Performance optimization
- [ ] Caching strategy with Redis
- [ ] Monitoring and logging
- [ ] Beta testing program
- [ ] Documentation
- [ ] Marketing landing page
- [ ] Production deployment

## API Design Considerations

### RESTful Endpoints Structure
```
/api/v1/
├── auth/
├── users/
├── groups/
├── subscriptions/
├── campaigns/
├── sessions/
├── sheets/
├── maps/
├── assets/
└── community/
```

### Real-time Features (SignalR)
- Live session updates
- Collaborative map editing
- Chat during sessions
- Initiative tracker updates

## Security Considerations
- JWT authentication with refresh tokens
- Rate limiting per user/IP
- Row-level security for multi-tenant data
- Input validation and sanitization
- HTTPS everywhere
- CORS configuration
- GDPR compliance for user data

## Cost Estimation (Monthly)

### Development Environment
- Azure/AWS resources: $50-100
- Domain and SSL: $10-15

### Production (Starting)
- 2-3 App Service instances: $150-250
- Managed PostgreSQL: $100-150
- Redis cache: $50-100
- Storage: $20-50
- CDN: $20-50
- Total: ~$340-600/month

### Scaled Production
- Kubernetes cluster: $300-500
- Database replicas: $200-300
- Redis cluster: $150-200
- Total: ~$650-1000/month

## Next Steps

1. **Week 1**: Set up development environment, create GitHub repo, configure CI/CD
2. **Week 2**: Implement authentication and basic user management
3. **Week 3**: Build group system with invitations
4. **Week 4**: Integrate Stripe for subscriptions
5. **Week 5**: Start character sheet designer MVP

## Useful Resources
- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core)
- [Next.js Documentation](https://nextjs.org/docs)
- [Stripe .NET SDK](https://github.com/stripe/stripe-dotnet)
- [SignalR Documentation](https://learn.microsoft.com/en-us/aspnet/core/signalr)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)

Would you like me to elaborate on any specific aspect of this plan?