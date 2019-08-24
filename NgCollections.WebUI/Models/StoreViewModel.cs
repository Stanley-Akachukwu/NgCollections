using NgCollections.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NgCollections.WebUI.Models
{
    public class StoreViewModel
    {
      public  NgPageWriteup Pagewritups { get; set; }
       public List<Product> TopRowProducts { get; set; } // sort and return 4
      public Product FashionTrend { get; set; } //sort and return 1
        public List<Product> ExclusiveRowOne { get; set; } //sort and return 4
        public List<Product> ExclusiveRowTwo { get; set; } //sort and return 4
        public List<Product> ExclusiveRowThree { get; set; } //sort and return 4
        public List<Blog> Blogs { get; set; }
    }
}