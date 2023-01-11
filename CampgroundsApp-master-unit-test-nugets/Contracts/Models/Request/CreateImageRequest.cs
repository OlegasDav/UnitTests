using System;

namespace Contracts.Models.Request
{
    public class CreateImageRequest
    {
        public Guid CampgroundId { get; set; }

        public string Url { get; set; }
    }
}