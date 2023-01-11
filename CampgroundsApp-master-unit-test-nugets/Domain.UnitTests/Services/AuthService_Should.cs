using AutoFixture;
using AutoFixture.Xunit2;
using Contracts.Models.Request;
using Contracts.Models.Response;
using Domain.Clients.Firebase;
using Domain.Clients.Firebase.Models;
using Domain.Services;
using Domain.UnitTests.Attributes;
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

        //[Fact]
        [Theory]
        [AutoMoqData]
        public async Task SignUpAsync_WithSingUpRequest_ReturnsSignUpResponse(
            SignUpRequest signUpRequest,
            FirebaseSignUpResponse firebaseSingUpResponse,
            [Frozen] Mock<IFirebaseClient> fireBaseClientMock,
            [Frozen] Mock<IUsersRepository> userRepositoryMock,
            AuthService sut)
        {
            // Arrange
            firebaseSingUpResponse.Email = signUpRequest.Email;

            fireBaseClientMock
                .Setup(firebaseClient => firebaseClient
                .SignUpAsync(signUpRequest.Email, signUpRequest.Password))
                .ReturnsAsync(firebaseSingUpResponse);

            // Act
            var result = await sut.SignUpAsync(signUpRequest);

            //galima palyginti objektus per memberius
            //result.Should().BeEquivalentTo(result, options => options.ComparingByMembers<SignUpResponse>());

            result.IdToken.Should().BeEquivalentTo(firebaseSingUpResponse.IdToken);
            result.Email.Should().BeEquivalentTo(firebaseSingUpResponse.Email);
            result.Email.Should().BeEquivalentTo(signUpRequest.Email);
            result.Username.Should().BeEquivalentTo(signUpRequest.Username);
            result.DateCreated.GetType().Should().Be<DateTime>();
            result.Id.GetType().Should().Be<Guid>();

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
        [Theory]
        [AutoMoqData]
        public async Task SignInAsync_WithSingInRequest_ReturnsSignInResponse(
            [Frozen] Mock<IFirebaseClient> fireBaseClientMock,
            [Frozen] Mock<IUsersRepository> userRepositoryMock,
            SignInRequest signInRequest,
            FirebaseSignInResponse firebaseSingInResponse,
            UserReadModel userReadModel,
            AuthService sut)
        {
            // Arrange
            firebaseSingInResponse.Email = signInRequest.Email;
            
            userReadModel.FirebaseId = firebaseSingInResponse.FirebaseId;
            userReadModel.Email = firebaseSingInResponse.Email;

            fireBaseClientMock
                .Setup(firebaseClient => firebaseClient
                .SignInAsync(signInRequest.Email, signInRequest.Password))
                .ReturnsAsync(firebaseSingInResponse);

            userRepositoryMock
                .Setup(userRepository => userRepository
                .GetAsync(firebaseSingInResponse.FirebaseId))
                .ReturnsAsync(userReadModel);

            // Act
            var result = await sut.SignInAsync(signInRequest);

            // Assert
            userReadModel.Username.Should().BeEquivalentTo(result.Username);
            userReadModel.Email.Should().BeEquivalentTo(result.Email);
            firebaseSingInResponse.IdToken.Should().BeEquivalentTo(result.IdToken);

            fireBaseClientMock
                .Verify(firebaseClient => firebaseClient
                .SignInAsync(signInRequest.Email, signInRequest.Password), Times.Once);

            userRepositoryMock
                .Verify(userRepository => userRepository
                .GetAsync(firebaseSingInResponse.FirebaseId), Times.Once);
        }
    }
}
