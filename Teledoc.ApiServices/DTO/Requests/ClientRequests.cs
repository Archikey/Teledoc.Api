using System.ComponentModel.DataAnnotations;
using Teledoc.ApiServices.DataBase.Entities;
using Teledoc.ApiServices.Validators;

namespace Teledoc.ApiServices.DTO.Requests
{
    public sealed class ClientRequests
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Inn]
        public string TaxpayerIndividualNumber { get; set; } = string.Empty;

        [Required]
        [EnumDataType(typeof(ClientType))]
        public ClientType TypeRole { get; set; }

        public List<FounderRequests> Founders { get; set; } = [];
    }

    public enum ClientType
    {
        LegalEntity = 1,
        IndividualEntrepreneur = 2
    }
}
