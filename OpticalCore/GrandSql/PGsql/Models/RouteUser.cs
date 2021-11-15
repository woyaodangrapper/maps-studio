using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace GrandSql.PGsql.Models
{
    public partial class RouteUser
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string RouteId { get; set; }
        public DateTime? Createtime { get; set; }
    }
}
