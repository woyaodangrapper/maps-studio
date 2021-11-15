using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace GrandSql.PGsql.Models
{
    public partial class Keyareas
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public short? Electronicfence { get; set; }
        public string Matrixcoordinates { get; set; }
        public string Fencestyle { get; set; }
        public string Fencecolor { get; set; }
        public string Matrixcolor { get; set; }
        public string UserId { get; set; }
    }
}
