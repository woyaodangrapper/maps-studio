using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace GrandSql.PGsql.Models
{
    public partial class Marker
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Picture { get; set; }
        public string Pointlist { get; set; }
        public string Style { get; set; }
        public string Special { get; set; }
        public DateTime? Createtime { get; set; }
    }
}
