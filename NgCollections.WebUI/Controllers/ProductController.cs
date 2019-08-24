using NgCollections.Domain.Abstract;
using NgCollections.Domain.Entities;
using NgCollections.WebUI.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NgCollections.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository _productRepository;
        private NgCollectionObjectMapper mapper = new NgCollectionObjectMapper();
        public int PageSize = 1;
        
        public ProductController(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }
        public ActionResult List(int? categoryId, int? id)
        {
            int navId = 0;
           List <Product> catproducts = (List<Product>)Session["catproducts"];
            if (catproducts == null)
            {
               IEnumerable<Product> productsByCategory = _productRepository.Products.Where(p => p.CategoryId == categoryId).ToList();
                catproducts = new List<Product>();
                foreach (var p in productsByCategory)
                {
                    Product navproduct = new Product();
                    navproduct = p;
                    navId = navId + 1;
                    navproduct.NavId = navId;
                    catproducts.Add(navproduct);
                }
                Session["catproducts"] = catproducts;
            }
            Product product = new Product();
            if (id == null)
            {
                product = catproducts.First();
                ViewBag.navId = product.NavId;
            }
            else
            {
                Product lastProduct = catproducts.Last();
                product = catproducts.Where(p => p.NavId == id).FirstOrDefault();
                ViewBag.navId = product.NavId;
                if (product== lastProduct)
                {
                    ViewBag.lastProduct = product;
                }
            }
            ProductListViewModel model = new ProductListViewModel
            {
                Product = product,
            };
            return PartialView("~/Views/Product/_List.cshtml", model);
        }
       
        public ActionResult NewProductDetails(int? id)
        {
            Session["detailVM"] = null;
            ProductDetailVM detailVM = new ProductDetailVM();
            if (id > 0 )
            {
                Product product = _productRepository.Products.FirstOrDefault(p => p.ProductID == id);
                detailVM = new ProductDetailVM();
                detailVM.SelectedProduct = product;
                detailVM.RelatedProducts = _productRepository.Products.Where(p => p.CategoryId == product.CategoryId).OrderBy(p => p.ProductID).Take(6).ToList();
                detailVM.CategoryName = _productRepository.Categories.FirstOrDefault(c => c.Id == product.CategoryId).Name;
                detailVM.NewProducts = _productRepository.Products.OrderByDescending(p => p.ProductID).Take(4).ToList();
                detailVM.HasEmptyRelatedProducts = true;
                Session["detailVM"] = detailVM;
            }
            return PartialView("~/Views/Product/_productDetails.cshtml", detailVM);

        }
        public ActionResult ProductDetails(int? id)
        {
            id = id < 0 ? 1 : id;
            ProductDetailVM detailVM = new ProductDetailVM();
            detailVM = (ProductDetailVM)Session["detailVM"];
            Product product = _productRepository.Products.FirstOrDefault(p => p.ProductID == id);
            if (detailVM==null)
            {
                if (product != null)
                {
                    detailVM = new ProductDetailVM();
                    detailVM.SelectedProduct = product;
                    detailVM.RelatedProducts = _productRepository.Products.Where(p => p.CategoryId == product.CategoryId).OrderBy(p => p.ProductID).Take(6).ToList();
                    detailVM.CategoryName = _productRepository.Categories.FirstOrDefault(c => c.Id == product.CategoryId).Name;
                    detailVM.NewProducts = _productRepository.Products.OrderByDescending(p => p.ProductID).Take(4).ToList();
                    detailVM.HasEmptyRelatedProducts = true;
                }
                Session["detailVM"] = detailVM; 
            }
            else
            {
                detailVM = (ProductDetailVM)Session["detailVM"];
                var moreRelatedProducts= _productRepository.Products.Where(p => p.CategoryId == detailVM.SelectedProduct.CategoryId && p.ProductID> id)
                    .OrderBy(p => p.ProductID).Take(6).ToList();
                if (moreRelatedProducts.Count > 0)
                {
                    detailVM.RelatedProducts = moreRelatedProducts;
                    detailVM.HasEmptyRelatedProducts = true;
                }
                else
                {
                    detailVM.RelatedProducts = _productRepository.Products.Where(p => p.CategoryId == product.CategoryId).OrderBy(p => p.ProductID).Take(6).ToList();
                    detailVM.HasEmptyRelatedProducts = false;
                }
                Session["detailVM"] = detailVM;
            }
            return PartialView("~/Views/Product/_productDetails.cshtml", detailVM);
        }
         
    }
}