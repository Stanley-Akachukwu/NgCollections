using NgCollections.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NgCollections.WebUI.Models
{
    public class ProductDetailVM
    {
        public Product SelectedProduct { get; set; }  
        public List<Product> RelatedProducts { get; set; }
        public List<Product> NewProducts { get; set; }
        public string CategoryName { get; set; }
        public bool HasEmptyRelatedProducts { get; set; }
    }
}