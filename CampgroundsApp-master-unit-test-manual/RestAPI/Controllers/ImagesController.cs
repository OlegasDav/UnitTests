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
    [Route("images")]
    public class ImagesController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IImagesRepository _imagesRepository;
        private readonly ICampgroundsRepository _campgroundsRepository;

        public ImagesController(
            IUsersRepository usersRepository, 
            IImagesRepository imagesRepository,
            ICampgroundsRepository campgroundsRepository)
        {
            _usersRepository = usersRepository;
            _imagesRepository = imagesRepository;
            _campgroundsRepository = campgroundsRepository;
        }
        
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CreateImageResponse>> Create(CreateImageRequest request)
        {
            var firebaseId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id").Value;
            var user = await _usersRepository.GetAsync(firebaseId);
            
            var campground = await _campgroundsRepository.GetAsync(request.CampgroundId);

            if (campground is null)
            {
                return NotFound("Campground not found");
            }
            
            if (campground.UserId != user.Id)
            {
                return Unauthorized($"User is unauthorized to add image to this campground: {request.CampgroundId}");
            }
            
            var imageReadModel = new ImageReadModel
            {
                Id = Guid.NewGuid(),
                CampgroundId = request.CampgroundId,
                Url = request.Url
            };

            await _imagesRepository.SaveAsync(imageReadModel);

            return Ok(new CreateImageResponse
            {
                Id = imageReadModel.Id,
                CampgroundId = imageReadModel.CampgroundId,
                Url = imageReadModel.Url
            });
        }
    }
}