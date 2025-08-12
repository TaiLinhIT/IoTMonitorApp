using TypeGen.Core.TypeAnnotations;

namespace IoTMonitorApp.API.Models
{
    [ExportTsClass]
    public class Brand : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
