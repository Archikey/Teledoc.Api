using System.ComponentModel.DataAnnotations;
using Teledoc.ApiServices.Validators;

namespace Teledoc.ApiServices.DTO.Responses
{
    public class FounderResponse
    {
        public int Id { get; set; }
        public string TaxpayerIndividualNumber { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
