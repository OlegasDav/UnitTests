using System.Threading.Tasks;
using Persistence.Models;

namespace Persistence.Repositories
{
    public interface IUsersRepository
    {
        Task<int> SaveAsync(UserReadModel model);
        
        Task<UserReadModel> GetAsync(string firebaseId);
    }
}