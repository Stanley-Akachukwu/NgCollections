using NgCollections.Domain.Abstract;
using NgCollections.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgCollections.Domain.Concrete
{
    public class EFProductRepository : IProductRepository
    {
        private EFDbContext context = new EFDbContext();
        public IEnumerable<Product> Products
        {
            get { return context.Products; }
        }

        public IEnumerable<Category> Categories  
        {
            get { return context.Categories; }
         }
       
        public Product SaveProduct(Product product)
        {
            if (product.ProductID == 0)
            {
                context.Products.Add(product);
            }
            else
            {
                Product dbEntry = context.Products.Find(product.ProductID);
                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.CategoryId = product.CategoryId;
                    dbEntry.AvailableSizes = product.AvailableSizes;
                    dbEntry.ProductNumberSize = product.ProductNumberSize;
                }
            }
            context.SaveChanges();
            return product;
        }
        public Product DeleteProduct(int productID)
        {
            Product dbEntry = context.Products.Find(productID);
            if (dbEntry != null)
            {
                context.Products.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        public void SaveWriteUps(NgPageWriteup writeup)
        {
            if (writeup.Id == 0)
            {
                context.NgPageWriteups.Add(writeup);
            }
            else
            {
                NgPageWriteup dbEntry = context.NgPageWriteups.Find(writeup.Id);
                if (dbEntry != null)
                {
                    dbEntry.ExclusiveProductText = writeup.ExclusiveProductText;
                    dbEntry.FashionTrendsText = writeup.FashionTrendsText;
                    dbEntry.TopNgCollectionText = writeup.TopNgCollectionText;
                    
                }
            }
            context.SaveChanges();
        }

        public NgPageWriteup GetWriteUps()
        {
            NgPageWriteup writeup =context.NgPageWriteups.Where(c=>c.Id==2).FirstOrDefault();
            if (writeup != null)
                return writeup;
            return null;
        }
 
    }
    }
