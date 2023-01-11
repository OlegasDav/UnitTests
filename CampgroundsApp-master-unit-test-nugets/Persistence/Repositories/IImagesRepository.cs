using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence.Models;

namespace Persistence.Repositories
{
    public interface IImagesRepository
    {
        Task<int> SaveAsync(ImageReadModel model);
        
        Task<IEnumerable<ImageReadModel>> GetAsync(Guid campgroundId);
        
        Task<IEnumerable<ImageReadModel>> GetAsync();

        Task<int> DeleteByIdAsync(Guid id);
        
        Task<int> DeleteByCampgroundIdAsync(Guid campgroundId);
    }
}