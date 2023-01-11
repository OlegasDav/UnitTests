using AutoFixture;
using AutoFixture.Xunit2;
using Contracts.Models.Request;
using Contracts.Models.Response;
using Domain.Clients.Firebase;
using Domain.Clients.Firebase.Models;
using Domain.Services;
using FluentAssertions;
using Moq;
using Persistence.Models;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Domain.UnitTests.Services
{
    public class AuthService_Should
    {
        //Given_When_Then

        [Fact]
        public async Task SignUpAsync_WithSingUpRequest_ReturnsSignUpResponse()
        {
            // Arrange

            var fireBaseClientMock = new Mock<IFirebaseClient>();
            var userRepositoryMock = new Mock<IUsersRepository>();

            var signUpRequest = new SignUpRequest
            {
                Username = Guid.NewGuid().ToString(),
                Email = Guid.NewGuid().ToString(),
                Password = Guid.NewGuid().ToString()
            };

            var firebaseSingUpResponse = new FirebaseSignUpResponse
            {
                IdToken = Guid.NewGuid().ToString(),
                Email = signUpRequest.Email,
                FirebaseId = Guid.NewGuid().ToString()
            };

            fireBaseClientMock
                .Setup(firebaseClient => firebaseClient
                .SignUpAsync(signUpRequest.Email, signUpRequest.Password))
                .ReturnsAsync(firebaseSingUpResponse);

            //sut - system under test
            var sut = new AuthService(fireBaseClientMock.Object, userRepositoryMock.Object);

            // Act
            var result = await sut.SignUpAsync(signUpRequest);

            // Assert
            Assert.IsType<Guid>(result.Id);
            Assert.Equal(signUpRequest.Username, result.Username);
            Assert.Equal(firebaseSingUpResponse.Email, result.Email);
            Assert.Equal(firebaseSingUpResponse.IdToken, result.IdToken);
            Assert.IsType<DateTime>(result.DateCreated);

            fireBaseClientMock
                .Verify(firebaseClient => firebaseClient
                .SignUpAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            userRepositoryMock
                .Verify(userRepository => userRepository
                .SaveAsync(It.Is<UserReadModel>(user => user.FirebaseId.Equals(firebaseSingUpResponse.FirebaseId) &&
                user.Username.Equals(signUpRequest.Username) &&
                user.Email.Equals(firebaseSingUpResponse.Email))), Times.Once);
        }

        //SignIn_WithSingInRequest_ReturnsSignInResponse
        [Fact]
        public async Task SignInAsync_WithSingInRequest_ReturnsSignInResponse()
        {
            // Arrange

            var fireBaseClientMock = new Mock<IFirebaseClient>();
            var userRepositoryMock = new Mock<IUsersRepository>();

            var signInRequest = new SignInRequest
            {
                Email = Guid.NewGuid().ToString(),
                Password = Guid.NewGuid().ToString()
            };

            var firebaseSingInResponse = new FirebaseSignInResponse
            {
                IdToken = Guid.NewGuid().ToString(),
                Email = signInRequest.Email,
                FirebaseId = Guid.NewGuid().ToString()
            };

            var userReadModel = new UserReadModel
            {
                Id = Guid.NewGuid(),
                FirebaseId = firebaseSingInResponse.FirebaseId,
                Username = Guid.NewGuid().ToString(),
                Email = firebaseSingInResponse.Email,
                DateCreated = DateTime.Now
            };

            fireBaseClientMock
                .Setup(firebaseClient => firebaseClient
                .SignInAsync(signInRequest.Email, signInRequest.Password))
                .ReturnsAsync(firebaseSingInResponse);

            userRepositoryMock
                .Setup(userRepository => userRepository
                .GetAsync(firebaseSingInResponse.FirebaseId))
                .ReturnsAsync(userReadModel);

            var expectedResult = new SignInResponse
            {
                Username = userReadModel.Username,
                Email = userReadModel.Email,
                IdToken = firebaseSingInResponse.IdToken
            };

            //sut - system under test
            var sut = new AuthService(fireBaseClientMock.Object, userRepositoryMock.Object);

            // Act
            var result = await sut.SignInAsync(signInRequest);

            // Assert
            Assert.Equal(expectedResult.Username, result.Username);
            Assert.Equal(expectedResult.Email, result.Email);
            Assert.Equal(expectedResult.IdToken, result.IdToken);

            fireBaseClientMock
                .Verify(firebaseClient => firebaseClient
                .SignInAsync(signInRequest.Email, signInRequest.Password), Times.Once);

            userRepositoryMock
                .Verify(userRepository => userRepository
                .GetAsync(firebaseSingInResponse.FirebaseId), Times.Once);
        }
    }
}
