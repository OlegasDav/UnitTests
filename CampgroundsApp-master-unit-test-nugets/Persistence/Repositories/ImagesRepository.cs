using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence.Models;

namespace Persistence.Repositories
{
    public class ImagesRepository : IImagesRepository
    {
        private const string TableName = "Images";
        
        private readonly ISqlClient _sqlClient;

        public ImagesRepository(ISqlClient sqlClient)
        {
            _sqlClient = sqlClient;
        }
        
        public Task<int> SaveAsync(ImageReadModel model)
        {
            var sql = $"INSERT INTO {TableName} (Id, CampgroundId, Url) VALUES (@Id, @CampgroundId, @Url)";

            return _sqlClient.ExecuteAsync(sql, model);
        }

        public Task<IEnumerable<ImageReadModel>> GetAsync(Guid campgroundId)
        {
            var sql = $"SELECT * FROM {TableName} WHERE CampgroundId = @CampgroundId";

            return _sqlClient.QueryAsync<ImageReadModel>(sql, new
            {
                CampgroundId = campgroundId
            });
        }

        public Task<IEnumerable<ImageReadModel>> GetAsync()
        {
            var sql = $"SELECT * FROM {TableName}";

            return _sqlClient.QueryAsync<ImageReadModel>(sql);
        }

        public Task<int> DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteByCampgroundIdAsync(Guid campgroundId)
        {
            throw new NotImplementedException();
        }
    }
}