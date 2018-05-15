using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebCoreMVC.Models
{
    public class UserInfo
    {
        private string _Name;
        private string _Password;
        private int _Sex;

        public string Name { get => _Name; set => _Name = value; }
        public string Password { get => _Password; set => _Password = value; }
        public int Sex { get => _Sex; set => _Sex = value; }
    }
}
