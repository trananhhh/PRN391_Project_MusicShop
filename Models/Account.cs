using System;
using System.Collections.Generic;

#nullable disable

namespace Project_MusicShop.Models
{
    public partial class Account
    {

        public Account()
        {
            Orders = new HashSet<Order>();
        }

        public int AccountId { get; set; }
        public string Username { get; set; }
        public string Pword { get; set; }
        public int Role { get; set; }

        public Account(string username, string pword, int role)
        {
            Username = username;
            Pword = pword;
            Role = role;
        }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
