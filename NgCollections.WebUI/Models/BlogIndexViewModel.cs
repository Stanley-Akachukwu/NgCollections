using NgCollections.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NgCollections.WebUI.Models
{
    public class BlogIndexViewModel
    {
        public List<Blog> RecentBlogs { get; set; }
        public List<Blog> PopularBlogs { get; set; }
        public List<Blog> IndexBlogs { get; set; }

    }
}