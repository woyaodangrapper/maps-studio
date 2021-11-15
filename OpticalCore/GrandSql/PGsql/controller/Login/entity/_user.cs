using System;
using System.Collections.Generic;
using System.Text;

namespace GrandSql.PGsql.controller.Login.entity
{
    public class _user
    {

        public class login
        {
            String username { get; set; }
            String psd { get; set; }
        }


        public class register
        {
            String nickname { get; set; }
            String username { get; set; }
            String psd { get; set; }
            String portrait { get; set; }
        }


        public class delete
        {
            String id { get; set; }
        }


        public class update
        {
            String id { get; set; }
            String nickname { get; set; }
            String psd { get; set; }
            String portrait { get; set; }
        }
    }
}
