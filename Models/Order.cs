using System;
using System.Collections.Generic;

#nullable disable

namespace Project_MusicShop.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public Order(int? accountId, DateTime orderDate, string firstName, string lastName, string address, string city, string state, string country, string phone, double? total)
        {
            AccountId = accountId;
            OrderDate = orderDate;
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            City = city;
            State = state;
            Country = country;
            Phone = phone;
            Total = total;
        }

        public int OrderId { get; set; }
        public int? AccountId { get; set; }
        public DateTime OrderDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public double? Total { get; set; }

        public virtual Account Account { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
