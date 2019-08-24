using NgCollections.Domain.Abstract;
using NgCollections.Domain.Entities;
using NgCollections.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace NgCollections.WebUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IBlogRepository _blogRepository;
        private NgCollectionObjectMapper mapper = new NgCollectionObjectMapper();
        public IProductRepository _productRepository;
        public HomeController(IBlogRepository blogRepository, IProductRepository productRepository)
        {
            _blogRepository = blogRepository;
            _productRepository = productRepository;
        }
        [AllowAnonymous]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.Client, Duration = 30)]
        public ActionResult Index()
        {
            Session["detailVM"] = null;
            StoreViewModel store = new StoreViewModel();
            IEnumerable<Product>products= (IEnumerable<Product>)Session["products"];
            if(products==null)
                products = _productRepository.Products.ToList();

            Session["products"]= products;
            store.ExclusiveRowOne = products.Take(12).ToList();
            store.ExclusiveRowTwo = products.Skip(12).Take(12).ToList();
            store.ExclusiveRowThree = products.Skip(25).Take(12).ToList();
            store.TopRowProducts = products.ToList();
            Random rnd = new Random();
            int r = rnd.Next(1, 50);
            if (r % 2 == 0)
            {
                store.FashionTrend = products.Where(p => p.ProductID == r).FirstOrDefault();
            }
            else
            {
                r = r + 1;
                store.FashionTrend = products.Where(p => p.ProductID == r).FirstOrDefault();
            }
            store.Pagewritups = _productRepository.GetWriteUps();
            store.Blogs = GetBlogs();
            return PartialView("~/Views/Home/_storePage.cshtml",store);

        }

        private List<Blog> GetBlogs()
        {
            List<Blog> blogs =_blogRepository.Blogs.OrderByDescending(b => b.Id).Take(5).ToList();
            List<Blog> lstBlogs = new List<Blog>();
            foreach (var blog in blogs)
            {
                int count = blogs.Where(b => b.ParentId == blog.Id).Count();
                if (count > 1)
                    blog.NumberOfComments = count + " comments";
                blog.NumberOfComments = count + " comment";
                lstBlogs.Add(blog);
            }
            return lstBlogs;
        }
    }
}