using System;

namespace Contracts.Models.Response
{
    public class SignUpResponse
    {
        public Guid Id { get; set; }

        public string IdToken { get; set; }
        
        public string Email { get; set; }
        
        public string Username { get; set; }

        public DateTime DateCreated { get; set; }
    }
}