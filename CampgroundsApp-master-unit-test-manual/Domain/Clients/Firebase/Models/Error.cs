using System.Text.Json.Serialization;

namespace Domain.Clients.Firebase.Models
{
    public class Error
    {
        [JsonPropertyName("code")]
        public int StatusCode { get; set; }
        
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}