using System.ComponentModel.DataAnnotations;
using TypeGen.Core.TypeAnnotations;

namespace IoTMonitorApp.API.Models
{
    [ExportTsClass]
    public class Cart : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool IsCheckOut { get; set; }
    }
}
