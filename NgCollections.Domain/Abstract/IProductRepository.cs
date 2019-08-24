using NgCollections.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgCollections.Domain.Abstract
{
    public interface IProductRepository
    {
        IEnumerable<Product> Products { get; }
        IEnumerable<Category> Categories { get; }
        Product SaveProduct(Product product);
        Product DeleteProduct(int productID);
        void SaveWriteUps(NgPageWriteup writeup);
        NgPageWriteup GetWriteUps();

    }
}
