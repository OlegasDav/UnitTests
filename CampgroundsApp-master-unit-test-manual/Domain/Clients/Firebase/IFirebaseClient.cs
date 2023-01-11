using System.Threading.Tasks;
using Domain.Clients.Firebase.Models;

namespace Domain.Clients.Firebase
{
    public interface IFirebaseClient
    {
        Task<FirebaseSignUpResponse> SignUpAsync(string email, string password);

        Task<FirebaseSignInResponse> SignInAsync(string email, string password);
    }
}