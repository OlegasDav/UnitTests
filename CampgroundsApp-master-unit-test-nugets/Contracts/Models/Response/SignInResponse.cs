namespace Contracts.Models.Response
{
    public class SignInResponse
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string IdToken { get; set; }
    }
}