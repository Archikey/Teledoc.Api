using System.ComponentModel.DataAnnotations;
using Teledoc.ApiServices.DTO.Requests;
using Teledoc.ApiServices.Validators;

namespace Teledoc.ApiServices.DTO.Responses
{
    public class ClientResponse
    {
        public int Id {  get; set; }
        public string Name { get; set; } = string.Empty;
        public string TaxpayerIndividualNumber { get; set; } = string.Empty;
        public ClientType TypeRole { get; set; }
        public List<FounderResponse> Founders { get; set; } = [];
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
