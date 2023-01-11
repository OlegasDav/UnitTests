using System.Text.Json.Serialization;

namespace Domain.Clients.Firebase.Models
{
    public class ErrorResponse
    {
        [JsonPropertyName("error")]
        public Error Error { get; set; }
    }
}