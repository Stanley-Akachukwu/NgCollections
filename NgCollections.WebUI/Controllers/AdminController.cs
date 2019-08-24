using NgCollections.Domain.Abstract;
using NgCollections.Domain.Entities;
using NgCollections.WebUI.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NgCollections.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private IBlogRepository _blogRepository;
        private NgCollectionObjectMapper mapper = new NgCollectionObjectMapper();
        public IProductRepository _productRepository;
        IOrderProcessor _orderProcessor;
        public int PageSize = 5;

        public AdminController(IBlogRepository blogRepository, IProductRepository productRepository, IOrderProcessor orderProcessor)
        {
            _blogRepository = blogRepository;
            _productRepository = productRepository;
            _orderProcessor = orderProcessor;
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        public ActionResult Orders()
        {
            int orderCount = _orderProcessor.Orders.Count();
            Session["orderCount"] = orderCount;
            return PartialView("~/Views/Admin/_Orders.cshtml", _orderProcessor.Orders.OrderByDescending(p => p.ProductID));
        }
        private List<BlogVM> GetBlogs()
        {
            List<BlogVM> blogVMs = new List<BlogVM>();
            List<Blog> blogs = _blogRepository.Blogs.OrderByDescending(b => b.Id).Take(4).ToList();
            foreach (var blog in blogs)
            {
                BlogVM blogVM = mapper.MapToBlogVM(blog);
                blogVMs.Add(blogVM);
            }
            return blogVMs;
        }
        public ActionResult List(int? searchId, int? id)
        {
             
            IEnumerable<Product> products = (IEnumerable<Product>)Session["products"];
            if (products == null)
                products = _productRepository.Products.ToList();
            Session["products"]= products;
            IEnumerable<Product> pageSize = new List<Product>();
            if (searchId != null)
            {
                pageSize = products.Where(p => p.ProductID == searchId);
            }
            else
            {
                if (id == null)
                {
                    pageSize = products.Take(5);
                    ViewBag.navId = pageSize.LastOrDefault().ProductID;
                    Product lastofList = pageSize.LastOrDefault();
                    Product LastofDb = products.LastOrDefault();
                    Product firstofDb = products.FirstOrDefault();
                   Session["current"] =  pageSize.LastOrDefault().ProductID;
                    ViewBag.lastofList = lastofList;
                    ViewBag.LastofDb = LastofDb;
                    ViewBag.firstofDb = firstofDb;
                }
                else if(id==0)
                {
                   int current = (int)Session["current"];
                    pageSize = products.Skip(((int)current - 5)).Take(5);
                    Session["current"] = pageSize.LastOrDefault().ProductID;
                    Product lastofList = pageSize.LastOrDefault();
                    Product LastofDb = products.LastOrDefault();
                    Product firstofDb = products.FirstOrDefault();
                    ViewBag.navId = pageSize.LastOrDefault().ProductID;
                     ViewBag.lastofList = lastofList;
                    ViewBag.LastofDb = LastofDb;
                    ViewBag.firstofDb = firstofDb;
                }
                else if (id == 1)
                {
                    int current = (int)Session["current"];
                    pageSize = products.Skip(((int)current)).Take(5);
                    Product lastofList = pageSize.LastOrDefault();
                    Product LastofDb = products.LastOrDefault();
                    Product firstofDb = products.FirstOrDefault();
                    Session["current"] = pageSize.LastOrDefault().ProductID;
                    ViewBag.lastofList = lastofList;
                    ViewBag.LastofDb = LastofDb;
                    ViewBag.firstofDb = firstofDb;

                }
            }
            AdminProductListViewModel model = new AdminProductListViewModel
            {
                Products = pageSize,
                ProductCount = products.Count(),
            };
            searchId = null;
            return PartialView("~/Views/Admin/_ProductList.cshtml", model);
        }
      
        public ActionResult OrderDetails()
        {
            TempData["errorMessage"] = "Not yet implemented";
            return RedirectToAction("Orders");
           // return PartialView("~/Views/Admin/_orderDetais.cshtml", new ProductVM());
        }
        public ActionResult Create()
        {
            return PartialView("~/Views/Admin/_addNewProduct.cshtml", new ProductVM() {
                AvailableSizes = "0,0,0",
                CategoryId = 1,
                Price = 100,
                ProductNumberSize = 25,
                Name = "bulk upload",
                Description = "bulk upload",
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductVM productVM)
        {
            Product product = new Product();
            if (ModelState.IsValid)
            {
                if (Request.Files.Count == 2)
                {
                    productVM.FrontImageUrl = 0 + "a";
                    productVM.BackImageUrl = 0 + "b";
                    product = _productRepository.SaveProduct(mapper.MapToProduct(productVM));
                }
                else
                {
                    TempData["errorMessage"] = "No product images found. You have to choose back and front for each product.";
                    return PartialView("~/Views/Admin/_addNewProduct.cshtml", productVM);
                }
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    var extension = Path.GetExtension(file.FileName);
                    if (file != null && file.ContentLength > 0)
                    {
                        if (i == 0)
                        {
                            var path = Path.Combine(Server.MapPath("~/Uploads/"), product.ProductID + "a" + extension);
                            file.SaveAs(path);
                        }
                        else
                        {
                            var path = Path.Combine(Server.MapPath("~/Uploads/"), product.ProductID + "b" + extension);
                            file.SaveAs(path);
                            product.FrontImageUrl = product.ProductID + "a" + extension;
                            product.BackImageUrl = product.ProductID + "b" + extension;
                            _productRepository.SaveProduct(product);
                        }

                    }
                }
                TempData["successMessage"] = string.Format("{0} has been saved.", "Bulk upload");
                return RedirectToAction("List");
            }
            else
            {
                TempData["errorMessage"] = string.Format("{0} has invalid information.", productVM.Name);
                return PartialView("~/Views/Admin/_addNewProduct.cshtml", productVM);
            }
        }
        public ActionResult Edit(int productId)
        {
            Product product = _productRepository.Products
            .FirstOrDefault(p => p.ProductID == productId);
            ProductVM productVM= new ProductVM();
            if (product!=null)
            {
                productVM= mapper.MapToProductVM(product);
            }
            return PartialView("~/Views/Admin/_editProduct.cshtml", productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductVM productVM)
        {
            Product product = new Product();
            if (ModelState.IsValid)
            {
                product= _productRepository.SaveProduct(mapper.MapToProduct(productVM));
                TempData["successMessage"] = string.Format("{0} has been saved.", "Bulk upload");
                return RedirectToAction("List");
            }
            else
            {
                TempData["errorMessage"] = string.Format("{0} has invalid information.", productVM.Name);
                return PartialView("~/Views/Admin/_editProduct.cshtml", productVM);
            }
        }

       

        [HttpPost]
        public ActionResult Delete(int productId)
        {
            Product deletedProduct = _productRepository.DeleteProduct(productId);
            if (deletedProduct != null)
            {
                string fullPathFrontImag = Request.MapPath("~/Uploads/" + deletedProduct+"a");
                if (System.IO.File.Exists(fullPathFrontImag))
                {
                    System.IO.File.Delete(fullPathFrontImag);
                }
                string fullPathBackImage = Request.MapPath("~/Uploads/" + deletedProduct + "b");
                if (System.IO.File.Exists(fullPathBackImage))
                {
                    System.IO.File.Delete(fullPathBackImage);
                }
                TempData["successMessage"] = string.Format("{0} was deleted",
                deletedProduct.Name);
            }
            return RedirectToAction("List");
        }
        [HttpPost]
        public ActionResult DeleteOrder(int orderid)
        {
            Order deletedOrder = _orderProcessor.DeleteOrder(orderid);
            if (deletedOrder != null)
            {               
                TempData["successMessage"] = string.Format("{0} order was deleted",
                deletedOrder.CustomerName);
            }
            return RedirectToAction("Orders");
        }
        public ActionResult EditNgPageWriteups()
        {
            NgPageWriteup writeup = _productRepository.GetWriteUps();
            if(writeup!=null)
                return PartialView("~/Views/Admin/_editPageWriteup.cshtml", writeup);
          return  PartialView("~/Views/Admin/_editPageWriteup.cshtml", new NgPageWriteup());

        }
        [HttpPost]
        public ActionResult EditNgPageWriteups(NgPageWriteup ngPageWriteup)
        {
            if (ModelState.IsValid)
            {
                
                _productRepository.SaveWriteUps(ngPageWriteup);
                TempData["successMessage"] = string.Format("Page writeup has been saved");
                return RedirectToAction("Index");
            }
            else
            {
                return View(ngPageWriteup);
            }
        }
    }
}