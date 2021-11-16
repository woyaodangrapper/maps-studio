using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace GrandSql.PGsql.Models
{
    public partial class Users
    {
        public string Id { get; set; }
        public string Nickname { get; set; }
        public string Username { get; set; }
        public string Psd { get; set; }
        public string Portrait { get; set; }
        public string Salt { get; set; }
        public short? Isadministrator { get; set; }
        public DateTime? Createtime { get; set; }
    }
}
