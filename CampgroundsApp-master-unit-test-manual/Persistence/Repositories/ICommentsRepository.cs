using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence.Models;

namespace Persistence.Repositories
{
    public interface ICommentsRepository
    {
        Task<int> SaveOrUpdateAsync(CommentReadModel model);
        
        Task<IEnumerable<CommentReadModel>> GetAsync(Guid campgroundId);

        Task<IEnumerable<CommentReadModel>> GetAsync(Guid id, Guid userId);
        
        Task<int> DeleteAsync(Guid id, Guid userId);
    }
}