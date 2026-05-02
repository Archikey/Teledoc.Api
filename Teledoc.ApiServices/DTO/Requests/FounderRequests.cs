using System.ComponentModel.DataAnnotations;
using Teledoc.ApiServices.Validators;

namespace Teledoc.ApiServices.DTO.Requests
{
    public sealed class FounderRequests
    {

        [Required]
        [Inn]
        public string TaxpayerIndividualNumber { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [StringLength(50)]
        public string? MiddleName { get; set; }
    }
}
