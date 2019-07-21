using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrdersManagement.Models
{
    public class IpAddress
    {
        [Key]
        public string Ip { get; set; }
        public int OrdersCount { get; set; }

        //public IList<Order> Orders { get; set; }
    }
}