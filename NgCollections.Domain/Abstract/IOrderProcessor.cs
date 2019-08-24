using NgCollections.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgCollections.Domain.Abstract
{
   public interface IOrderProcessor
    {
       IEnumerable<Order> Orders { get; }
        void ProcessOrder(Cart cart, ShippingDetails shippingDetails);
        void SaveOrder(List<Order> orders);
        Order DeleteOrder(int orderID);

        
    }
}
