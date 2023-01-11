using System;

namespace Contracts.Models.Response
{
    public class CommentResponse
    {
        public Guid Id { get; set; }
        
        public int Rating { get; set; }

        public string Text { get; set; }

        public Guid UserId { get; set; }

        public DateTime DateCreated { get; set; }
    }
}