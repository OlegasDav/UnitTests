using System.Threading.Tasks;
using Persistence.Models;

namespace Persistence.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private const string TableName = "Users";
        private readonly ISqlClient _sqlClient;

        public UsersRepository(ISqlClient sqlClient)
        {
            _sqlClient = sqlClient;
        }
        
        public Task<int> SaveAsync(UserReadModel model)
        {
            var sql = @$"INSERT INTO {TableName} (Id, FirebaseId, Username, Email, DateCreated) 
                        VALUES (@Id, @FirebaseId, @Username, @Email, @DateCreated)";

            return _sqlClient.ExecuteAsync(sql, model);
        }

        public Task<UserReadModel> GetAsync(string firebaseId)
        {
            var sql = $"SELECT * FROM {TableName} WHERE FirebaseId = @FirebaseId";

            return _sqlClient.QuerySingleOrDefaultAsync<UserReadModel>(sql, new
            {
                FirebaseId = firebaseId
            });
        }
    }
}