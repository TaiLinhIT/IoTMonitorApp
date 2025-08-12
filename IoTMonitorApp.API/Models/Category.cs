using System.ComponentModel.DataAnnotations;
using TypeGen.Core.TypeAnnotations;

namespace IoTMonitorApp.API.Models
{
    [ExportTsClass]
    public class Category : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
