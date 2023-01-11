namespace Contracts.Models.Request
{
    public class CreateCampgroundRequest
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }
    }
}