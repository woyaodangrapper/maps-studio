using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace GrandSql.PGsql.Models
{
    public partial class Facedata
    {
        public string Id { get; set; }
        public string Picturepath { get; set; }
        public string Usercode { get; set; }
        public string Username { get; set; }
        public DateTime? Createtime { get; set; }
    }
}
