using Backend.Application.IRepositories;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;

namespace Backend.Infrastructure.Repositories
{
    public class ReturnRequestRepository : BaseRepository<ReturnRequest>, IReturnRequestRepository
    {
        public ReturnRequestRepository(AssetContext dbContext) : base(dbContext)
        {
        }

    }
}
