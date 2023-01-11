using System;

namespace Persistence.Models
{
    public class UserReadModel
    {
        public Guid Id { get; set; }

        public string FirebaseId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public DateTime DateCreated { get; set; }
    }
}