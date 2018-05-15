using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebAPI.Models
{
    public class UserInfo
    {
        private int id = 0;
        private string name = "";
        private string phone = "";

        public int Id { get ; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}