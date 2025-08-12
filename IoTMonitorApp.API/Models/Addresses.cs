using System.ComponentModel.DataAnnotations;
using TypeGen.Core.TypeAnnotations;

namespace IoTMonitorApp.API.Models
{
    [ExportTsClass]
    public class Addresses : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string? PostalCode { get; set; }
        public string Country { get; set; }
        public bool IsDefault { get; set; }
    }
}
