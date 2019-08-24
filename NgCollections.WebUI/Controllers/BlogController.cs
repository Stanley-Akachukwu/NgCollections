using NgCollections.Domain.Abstract;
using NgCollections.Domain.Entities;
using NgCollections.WebUI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NgCollections.WebUI.Controllers
{
    public class BlogController : Controller
    {
        private IBlogRepository repository;
        private NgCollectionObjectMapper mapper = new NgCollectionObjectMapper();
        public BlogController(IBlogRepository repo)
        {
            repository = repo;
        }
        public ActionResult Blogs()
        {
            BlogIndexViewModel model = new BlogIndexViewModel();
            model.IndexBlogs = repository.Blogs.OrderByDescending(b => b.Id).Take(4).ToList();
            model.PopularBlogs= repository.Blogs.Where(b => b.Popularity > 3).Take(4).ToList();
            model.RecentBlogs = repository.Blogs.OrderByDescending(b => b.Id).Take(9).Reverse().ToList();
            model.RecentBlogs = model.RecentBlogs.Take(5).ToList();
            return PartialView("~/Views/Blog/_blogs.cshtml", model);
        }
        
        public ActionResult BlogDetails(int? id)
        {
            Blog blog = repository.Blogs
           .FirstOrDefault(p => p.Id == id);
            BlogVM blogVM = new BlogVM();
            if (blog != null)
            {
                blogVM = mapper.MapToBlogVM(blog);
                blogVM.GetComments = repository.Blogs.Where(c => c.ParentId == blog.Id).ToList();
            }
            return PartialView("~/Views/Blog/_blogDetails.cshtml", blogVM);
        }
        public ActionResult List()
        {
            return PartialView("~/Views/Blog/_blogList.cshtml", repository.Blogs);
        }
        public ActionResult Create()
        {
            return PartialView("~/Views/Blog/_addNewBlog.cshtml", new BlogVM());
        }
        public ActionResult Edit(int id)
        {
            Blog blog = repository.Blogs
            .FirstOrDefault(p => p.Id == id);
            BlogVM blogVM = new BlogVM();
            if (blog != null)
            {
                blogVM = mapper.MapToBlogVM(blog);
            }
            return PartialView("~/Views/Blog/_editBlog.cshtml", blogVM);
        }
        [HttpPost]
        public ActionResult Create(BlogVM blogVM)
        {
            Blog blog = new Blog();

            if (ModelState.IsValid)
            {

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    blogVM.ImageUrl = "oa";
                    blog = repository.SaveBlog(mapper.MapToBlog(blogVM));
                    var file = Request.Files[i];
                    var extension = Path.GetExtension(file.FileName);
                    if (file != null && file.ContentLength > 0)
                    {
                        if (i == 0)
                        {
                            var path = Path.Combine(Server.MapPath("~/Uploads/Blogs/"), blog.Id  + extension);
                            file.SaveAs(path);
                            blog.ImageUrl = blog.Id + extension;
                            repository.SaveBlog(blog);
                        }

                    }
                }
                TempData["successMessage"] = string.Format("{0} has been saved.", blog.Title);
                return PartialView("~/Views/Blog/_blogList.cshtml", repository.Blogs);
            }
            else
            {
                TempData["errorMessage"] = string.Format("{0} has invalid information.", blog.Title);
                return PartialView("~/Views/Blog/_editBlog.cshtml", blogVM);
            }

        }
        [HttpPost]
        public ActionResult Edit(BlogVM blogVM)
        {
            Blog blog = new Blog();

            if (ModelState.IsValid)
            {
                bool isComment = false;
                blog = repository.SaveBlog(mapper.MapToBlog(blogVM), isComment);
                TempData["successMessage"] = string.Format("{0} has been saved.", blog.Title);
                return PartialView("~/Views/Blog/_blogList.cshtml", repository.Blogs);
            }
            else
            {
                TempData["errorMessage"] = string.Format("{0} has invalid information.", blog.Title);
                return PartialView("~/Views/Blog/_editBlog.cshtml", blogVM);
            }

        }
        [HttpPost]
        public ActionResult AddBlogComment(string name, string email, string comment, int parentId)
        {
            Blog b = repository.Blogs.Where(p=>p.Id==parentId).FirstOrDefault();
            BlogVM blogVM = new BlogVM();
            blogVM.Author = name;
            blogVM.AuthorEmail = email;
            blogVM.Content = comment;
            blogVM.Title = b.Title;
           
            if (ModelState.IsValid)
            {
                bool isComment = true; 
                repository.SaveBlog(mapper.MapToBlog(blogVM), isComment);
                TempData["successMessage"] = string.Format("{0} has been saved", blogVM.Title);
                return RedirectToAction("Index","Home");
            }
            else
            {
                return View(blogVM);
            }
        }
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            Blog deletedProduct = repository.DeleteBlog(Id);
            if (deletedProduct != null)
            {
                TempData["successMessage"] = string.Format("{0} was deleted",
                deletedProduct.Title);
            }
            return RedirectToAction("List");
        }
         
    }
}