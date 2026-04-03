using CommunityCarApi.Domain.Entities;
using CommunityCarApi.Infrastructure.Data;

namespace CommunityCarApi.Infrastructure.Repositories;

public class ReviewRepository : Repository<Review>, IReviewRepository
{
    public ReviewRepository(ApplicationDbContext context) : base(context)
    {
    }
}
