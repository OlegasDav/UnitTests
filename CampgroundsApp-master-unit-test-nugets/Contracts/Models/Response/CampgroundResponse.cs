using System;
using System.Collections.Generic;

namespace Contracts.Models.Response
{
    public class CampgroundResponse
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; }

        public IEnumerable<CommentResponse> Comments { get; set; }

        public IEnumerable<ImageResponse> Images { get; set; }
    }
}