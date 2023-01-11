using System;
using System.ComponentModel.DataAnnotations;

namespace Contracts.Models.Request
{
    public class CreateCommentRequest
    {
        public Guid CampgroundId { get; set; }

        public string Text { get; set; }

        [Range(1, 5, ErrorMessage = "Rating should be between 1-5")]
        public int Rating { get; set; }
    }
}