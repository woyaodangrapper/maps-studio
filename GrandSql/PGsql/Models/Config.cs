using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace GrandSql.PGsql.Models
{
    public partial class Config
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string TitleList { get; set; }
        public string InitLanLong { get; set; }
        public string IconBase64 { get; set; }
        public short? Embedded { get; set; }
    }
}
