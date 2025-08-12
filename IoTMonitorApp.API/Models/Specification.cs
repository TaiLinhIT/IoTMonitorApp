using TypeGen.Core.TypeAnnotations;

namespace IoTMonitorApp.API.Models
{
    [ExportTsClass]
    public class Specification : BaseEntity
    {
        public int Id { get; set; }
        public int? Storage { get; set; }
        public double? SizeDisplay { get; set; }
        public string? Color { get; set; }
        public string? Material { get; set; }
        public double? Battery { get; set; }
    }
}
