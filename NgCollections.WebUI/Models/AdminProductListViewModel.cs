using NgCollections.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NgCollections.WebUI.Models
{
    public class AdminProductListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public bool? MinusSize { get; set; }
        public bool? PlusSize { get; set; }
        public int ProductCount { get; set; }
        public int? CategoryId { get; set; }

    }
}