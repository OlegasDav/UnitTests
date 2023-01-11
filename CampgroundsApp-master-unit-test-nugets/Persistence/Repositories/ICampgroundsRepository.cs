using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence.Models;

namespace Persistence.Repositories
{
    public interface ICampgroundsRepository
    {
        Task<int> SaveOrUpdateAsync(CampgroundReadModel model);

        Task<IEnumerable<CampgroundReadModel>> GetAllAsync();

        Task<CampgroundReadModel> GetAsync(Guid id);

        Task<int> DeleteAsync(Guid id);
    }
}