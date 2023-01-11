using System;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Models.Request;
using Contracts.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Models;
using Persistence.Repositories;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("comments")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsRepository _commentsRepository;
        private readonly ICampgroundsRepository _campgroundsRepository;
        private readonly IUsersRepository _usersRepository;

        public CommentsController(
            ICommentsRepository commentsRepository, 
            ICampgroundsRepository campgroundsRepository,
            IUsersRepository usersRepository)
        {
            _commentsRepository = commentsRepository;
            _campgroundsRepository = campgroundsRepository;
            _usersRepository = usersRepository;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CommentResponse>> Create(CreateCommentRequest request)
        {
            var firebaseId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id").Value;

            var user = await _usersRepository.GetAsync(firebaseId);
            
            var commentsReadModels = new CommentReadModel
            {
                Id = Guid.NewGuid(),
                CampgroundId = request.CampgroundId,
                Rating = request.Rating,
                Text = request.Text,
                UserId = user.Id,
                DateCreated = DateTime.Now
            };

            await _commentsRepository.SaveOrUpdateAsync(commentsReadModels);

            return Ok(new CommentResponse
            {
                Id = commentsReadModels.Id,
                Rating = commentsReadModels.Rating,
                Text = commentsReadModels.Text,
                UserId = commentsReadModels.UserId,
                DateCreated = commentsReadModels.DateCreated
            });
        }
    }
}