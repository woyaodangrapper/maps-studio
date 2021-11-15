using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace GrandSql.PGsql.Models
{
    public partial class Roaming
    {
        public string Id { get; set; }
        public string UserRoutedataId { get; set; }
        public float? Rate { get; set; }
        public string Thumbnail { get; set; }
        public short? Isend { get; set; }
        public DateTime? Createtime { get; set; }
    }
}
