using System.Threading.Tasks;
using Contracts.Models.Request;
using Contracts.Models.Response;

namespace Domain.Services
{
    public interface IAuthService
    {
        Task<SignUpResponse> SignUpAsync(SignUpRequest request);

        Task<SignInResponse> SignInAsync(SignInRequest request);
    }
}