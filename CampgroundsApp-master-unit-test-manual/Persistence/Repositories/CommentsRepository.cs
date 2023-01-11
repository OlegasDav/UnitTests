using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence.Models;

namespace Persistence.Repositories
{
    public class CommentsRepository : ICommentsRepository
    {
        private const string TableName = "Comments";
        
        private readonly ISqlClient _sqlClient;

        public CommentsRepository(ISqlClient sqlClient)
        {
            _sqlClient = sqlClient;
        }

        public Task<int> SaveOrUpdateAsync(CommentReadModel model)
        {
            var sql = @$"INSERT INTO {TableName} (Id, CampgroundId, UserId, Rating, Text, DateCreated) 
                        VALUES (@Id, @CampgroundId, @UserId, @Rating, @Text, @DateCreated)
                        ON DUPLICATE KEY UPDATE Rating = @Rating, Text = @Text";
            
            return _sqlClient.ExecuteAsync(sql, model);
        }

        public Task<IEnumerable<CommentReadModel>> GetAsync(Guid campgroundId)
        {
            var sql = $"SELECT * FROM {TableName} WHERE CampgroundId = @CampgroundId";

            return _sqlClient.QueryAsync<CommentReadModel>(sql, new
            {
                CampgroundId = campgroundId
            });
        }

        public Task<IEnumerable<CommentReadModel>> GetAsync(Guid id, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Guid id, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}