using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace GrandSql.PGsql.Models
{
    public partial class PersonnelPositioningdata
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string BasemapId { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public string Name { get; set; }
        public string Usercode { get; set; }
        public string Data { get; set; }
        public DateTime? Createtime { get; set; }
    }
}
