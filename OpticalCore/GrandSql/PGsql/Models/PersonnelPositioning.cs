using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace GrandSql.PGsql.Models
{
    public partial class PersonnelPositioning
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string KeyareasId { get; set; }
        public string Coordinatetype { get; set; }
        public string Offsetpoint { get; set; }
        public DateTime? Createtime { get; set; }
    }
}
