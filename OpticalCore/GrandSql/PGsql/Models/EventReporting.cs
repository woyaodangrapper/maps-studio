using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace GrandSql.PGsql.Models
{
    public partial class EventReporting
    {
        public string Id { get; set; }
        public string RoamingId { get; set; }
        public string Attachdata { get; set; }
        public int? Eventlevel { get; set; }
        public DateTime? Createtime { get; set; }
    }
}
