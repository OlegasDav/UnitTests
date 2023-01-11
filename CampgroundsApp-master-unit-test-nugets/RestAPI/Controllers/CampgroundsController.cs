using System;
using System.Collections.Generic;
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
    [Route("campgrounds")]
    public class CampgroundsController : ControllerBase
    {
        private readonly ICampgroundsRepository _campgroundsRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly ICommentsRepository _commentsRepository;
        private readonly IImagesRepository _imagesRepository;

        public CampgroundsController(
            ICampgroundsRepository campgroundsRepository, 
            IUsersRepository usersRepository,
            ICommentsRepository commentsRepository,
            IImagesRepository imagesRepository)
        {
            _campgroundsRepository = campgroundsRepository;
            _usersRepository = usersRepository;
            _commentsRepository = commentsRepository;
            _imagesRepository = imagesRepository;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<CampgroundResponse>> Get(Guid id)
        {
            var campground = await _campgroundsRepository.GetAsync(id);
            var comments = await _commentsRepository.GetAsync(id);
            var images = await _imagesRepository.GetAsync(id);

            var commentsResponse = comments.Select(comment => new CommentResponse
            {
                Id = comment.Id,
                Rating = comment.Rating,
                Text = comment.Text,
                UserId = comment.UserId,
                DateCreated = comment.DateCreated
            });
            
            var imagesResponse = images.Select(image => new ImageResponse
            {
                Id = image.Id,
                Url = image.Url
            });
            
            return Ok(new CampgroundResponse
            {
                Id = campground.Id,
                UserId = campground.UserId,
                Name = campground.Name,
                Price = campground.Price,
                Description = campground.Description,
                DateCreated = campground.DateCreated,
                Comments = commentsResponse,
                Images = imagesResponse
            });
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampgroundResponse>>> Get()
        {
            var campground = await _campgroundsRepository.GetAllAsync();
            var images = await _imagesRepository.GetAsync();

            var response = campground.Select(campground => new ShortCampgroundResponse
            {
                Id = campground.Id,
                UserId = campground.UserId,
                Name = campground.Name,
                Price = campground.Price,
                Description = campground.Description,
                DateCreated = campground.DateCreated,
                DefaultImageUrl = images.FirstOrDefault(image => image.CampgroundId == campground.Id)?.Url
            });
                
            return Ok(response);
        }
        
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CreateCampgroundResponse>> Create(CreateCampgroundRequest request)
        {
            var firebaseId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id").Value;

            var user = await _usersRepository.GetAsync(firebaseId);

            var campground = new CampgroundReadModel
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Name = request.Name,
                Price = request.Price,
                Description = request.Description,
                DateCreated = DateTime.Now
            };

            await _campgroundsRepository.SaveOrUpdateAsync(campground);

            return Ok(new CreateCampgroundResponse
            {
                Id = campground.Id,
                UserId = campground.UserId,
                Name = campground.Name,
                Price = campground.Price,
                Description = campground.Description,
                DateCreated = campground.DateCreated
            });
        }
    }
}