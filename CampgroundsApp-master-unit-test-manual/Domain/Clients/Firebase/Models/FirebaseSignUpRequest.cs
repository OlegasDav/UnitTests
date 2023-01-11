using System.Text.Json.Serialization;

namespace Domain.Clients.Firebase.Models
{
    public class FirebaseSignUpRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("returnSecureToken")]
        public bool ReturnSecureToken => true;
    }
}