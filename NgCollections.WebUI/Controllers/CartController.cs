using NgCollections.Domain.Abstract;
using NgCollections.Domain.Entities;
using NgCollections.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NgCollections.WebUI.Controllers
{
    public class CartController : Controller
    {

        private IProductRepository repository;
        private IOrderProcessor orderProcessor;
        public CartController(IProductRepository repo, IOrderProcessor proc)
        {
            repository = repo;
            orderProcessor = proc;
        }
        public ActionResult Index(Cart  cart,string returnUrl)
        {
            return PartialView("~/Views/Cart/_Index.cshtml", new CartIndexViewModel
            {
                ReturnUrl = returnUrl,
                Cart = cart
            });
        }
        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }
        public ActionResult Checkout()
        {
            ShippingDetails shippingDetails = new ShippingDetails();
            //shippingDetails.Email = "db@yahoo.com";
            //shippingDetails.Name = "Stan Dev";
            //shippingDetails.PhoneNumber = "080283772822";

            shippingDetails.Cart = GetCart();
            return PartialView("~/Views/Cart/_Checkout.cshtml", shippingDetails);
        }
        [HttpPost]
        public ActionResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
                shippingDetails.Cart = GetCart();
                return PartialView("~/Views/Cart/_Checkout.cshtml", shippingDetails);
            }
            if (ModelState.IsValid && shippingDetails.Email!=null && shippingDetails.PhoneNumber != null)
            {
                List<Order> orders = new List<Order>();
                foreach (var line in cart.Lines)
                {
                    Order order = new Order();
                    order.Amount = line.Quantity * line.Product.Price;
                    order.CustomerPhoneNumber = shippingDetails.PhoneNumber;
                    order.CustomerName = shippingDetails.Name;
                    order.OrderDate = DateTime.Now;
                    order.ProductID = line.Product.ProductID;
                    order.Quantity = line.Quantity;
                    order.CustomerEmail = shippingDetails.Email;
                    order.ProductName = line.Product.Name;
                    orders.Add(order);

                }
                orderProcessor.SaveOrder(orders);
                cart.Clear();
                return PartialView("~/Views/Cart/_Completed.cshtml", shippingDetails);
            }
            else
            {
                shippingDetails.Cart = GetCart();
                TempData["errorMessage"] = string.Format("Invalid details supplied! Kindly verify the email and phone number you entered.");
                return PartialView("~/Views/Cart/_Checkout.cshtml", shippingDetails);
            }
        }
        public RedirectToRouteResult AddToCart(Cart cart, int productId, string returnUrl)
        {
            Product product = repository.Products
            .FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId, string returnUrl)
        {
            Product product = repository.Products
            .FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        private Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
    }
}