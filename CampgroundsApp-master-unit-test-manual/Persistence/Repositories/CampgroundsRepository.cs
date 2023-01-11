using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence.Models;

namespace Persistence.Repositories
{
    public class CampgroundsRepository : ICampgroundsRepository
    {
        private const string CampgroundsTableName = "Campgrounds";
        private readonly ISqlClient _sqlClient;

        public CampgroundsRepository(ISqlClient sqlClient)
        {
            _sqlClient = sqlClient;
        }

        public Task<int> SaveOrUpdateAsync(CampgroundReadModel model)
        {
            var sql = @$"INSERT INTO {CampgroundsTableName} (Id, UserId, Name, Price, Description, DateCreated) 
                        VALUES (@Id, @UserId, @Name, @Price, @Description, @DateCreated)
                        ON DUPLICATE KEY UPDATE Name = @Name, Price = @Price, Description = @Description";

            return _sqlClient.ExecuteAsync(sql, model);
        }

        public Task<IEnumerable<CampgroundReadModel>> GetAllAsync()
        {
            var sql = $"SELECT * FROM {CampgroundsTableName}";

            return _sqlClient.QueryAsync<CampgroundReadModel>(sql);
        }

        public Task<CampgroundReadModel> GetAsync(Guid id)
        {
            var sql = $"SELECT * FROM {CampgroundsTableName} WHERE Id = @Id";

            return _sqlClient.QuerySingleOrDefaultAsync<CampgroundReadModel>(sql, new
            {
                Id = id
            });
        }

        public Task<int> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}