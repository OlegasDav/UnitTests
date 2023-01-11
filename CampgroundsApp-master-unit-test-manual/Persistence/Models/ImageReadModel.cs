using System;

namespace Persistence.Models
{
    public class ImageReadModel
    {
        public Guid Id { get; set; }

        public Guid CampgroundId { get; set; }

        public string Url { get; set; }
    }
}