using System;

namespace Contracts.Models.Response
{
    public class CreateImageResponse
    {
        public Guid Id { get; set; }
        
        public Guid CampgroundId { get; set; }

        public string Url { get; set; }
    }
}