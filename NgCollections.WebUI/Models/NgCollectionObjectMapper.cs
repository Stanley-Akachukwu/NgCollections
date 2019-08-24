using AutoMapper;
using NgCollections.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NgCollections.WebUI.Models
{
    public class NgCollectionObjectMapper
    {
        public Product MapToProduct(ProductVM productVM)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<ProductVM, Product>();
            });
            var mapper = config.CreateMapper();
            var product = mapper.Map<ProductVM, Product>(productVM);
            return product;
        }
        public ProductVM MapToProductVM(Product product)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Product,ProductVM>();
            });
            var mapper = config.CreateMapper();
            var productVM = mapper.Map<Product,ProductVM>(product);
            return productVM;
        }
        public Blog MapToBlog(BlogVM blogVM)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<BlogVM, Blog>();
            });
            var mapper = config.CreateMapper();
            var blog = mapper.Map<BlogVM, Blog>(blogVM);
            blog.EntryDate = DateTime.Now;
            return blog;
        }
        public BlogVM MapToBlogVM(Blog blog)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Blog, BlogVM>();
            });
            var mapper = config.CreateMapper();
            var blogVM = mapper.Map<Blog, BlogVM>(blog);
            return blogVM;
        }
    }
}
