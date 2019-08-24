using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgCollections.Domain.Entities
{
   public class Order
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerEmail { get; set; }
        public string ProductName { get; set; }
        public DateTime OrderDate { get; set; }
        public int Served { get; set; }
        public int Confirmed { get; set; }
    }
}
 