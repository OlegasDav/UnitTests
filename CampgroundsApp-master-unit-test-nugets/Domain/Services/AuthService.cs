using System;
using System.Threading.Tasks;
using Contracts.Models.Request;
using Contracts.Models.Response;
using Domain.Clients.Firebase;
using Domain.Clients.Firebase.Models;
using Persistence.Models;
using Persistence.Repositories;

namespace Domain.Services
{
    public class AuthService : IAuthService
    {
        private readonly IFirebaseClient _firebaseClient;
        private readonly IUsersRepository _usersRepository;

        public AuthService(IFirebaseClient firebaseClient, IUsersRepository usersRepository)
        {
            _firebaseClient = firebaseClient;
            _usersRepository = usersRepository;
        }
        
        public async Task<SignUpResponse> SignUpAsync(SignUpRequest request)
        {
            var user = await _firebaseClient.SignUpAsync(request.Email, request.Password);
                
            var userReadModel = new UserReadModel
            {
                Id = Guid.NewGuid(),
                FirebaseId = user.FirebaseId,
                Username = request.Username,
                Email = user.Email,
                DateCreated = DateTime.Now
            };

            await _usersRepository.SaveAsync(userReadModel);
            
            return new SignUpResponse
            {
                Id = userReadModel.Id,
                IdToken = user.IdToken,
                Email = userReadModel.Email,
                Username = userReadModel.Username,
                DateCreated = userReadModel.DateCreated
            };
        }

        public async Task<SignInResponse> SignInAsync(SignInRequest request)
        {
            var firebaseSignInResponse = await _firebaseClient.SignInAsync(request.Email, request.Password);

            var user = await _usersRepository.GetAsync(firebaseSignInResponse.FirebaseId);

            return new SignInResponse
            {
                Username = user.Username,
                Email = user.Email,
                IdToken = firebaseSignInResponse.IdToken
            };
        }
    }
}