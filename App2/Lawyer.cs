using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2
{
    public class Lawyer
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public Lawyer()
        { 
        }

        public Lawyer(string name,string phone,string email)
        {
            Name = name;
            Phone = phone;
            Email = email;
        }

    }
}
