using System;

namespace Contracts.Models.Response
{
    public class ShortCampgroundResponse
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; }
        
        public string DefaultImageUrl { get; set; }
    }
}