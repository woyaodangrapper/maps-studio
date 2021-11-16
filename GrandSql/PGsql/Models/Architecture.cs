using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace GrandSql.PGsql.Models
{
    public partial class Architecture
    {
        public string Id { get; set; }
        public string Pid { get; set; }
        public string RolseId { get; set; }
        public string Name { get; set; }
        public string ModelUrl { get; set; }
        public short? Selftest { get; set; }
        public DateTime? Createtime { get; set; }
    }
}
