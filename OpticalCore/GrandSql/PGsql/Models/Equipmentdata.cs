using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace GrandSql.PGsql.Models
{
    public partial class Equipmentdata
    {
        public string Equipmentcode { get; set; }
        public string ArchitectureId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public double? Size { get; set; }
        public string Model { get; set; }
        public string Config { get; set; }
        public DateTime? Createtime { get; set; }
    }
}
