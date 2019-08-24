using NgCollections.Domain.Abstract;
using NgCollections.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NgCollections.WebUI.Controllers
{
    public class AccountController : Controller
    {
        IAuthentication authProvider;
        public AccountController(IAuthentication auth)
        {
            authProvider = auth;
        }
        [AllowAnonymous]
        public ViewResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (authProvider.Authenticate(model.UserName, model.Password))
                {
                    if (authProvider.IsAdmin(model.UserName, model.Password))
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, false);
                        return Redirect(returnUrl ?? Url.Action("Index", "Admin"));
                    }
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    return Redirect(returnUrl ?? Url.Action("Index", "Home"));
                }
                else
                {
                    TempData["errorMessage"] = string.Format("Incorrect username or password");
                    ModelState.AddModelError("", "Incorrect username or password");
                    return View();
                }
            }
            else
            {
                TempData["errorMessage"] = string.Format("Ivalid username");
                return View();
            }
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("List", "Product");
        }
    }
}
