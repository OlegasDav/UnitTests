using System;

namespace Persistence.Models
{
    public class CampgroundReadModel
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; }
    }
}