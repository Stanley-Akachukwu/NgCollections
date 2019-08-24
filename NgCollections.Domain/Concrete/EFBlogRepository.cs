using NgCollections.Domain.Abstract;
using NgCollections.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgCollections.Domain.Concrete
{
    public class EFBlogRepository : IBlogRepository
    {
        private EFDbContext context = new EFDbContext();
        public IEnumerable<Blog> Blogs => context.Blogs;

        public Blog DeleteBlog(int id)
        {
            Blog dbblog = context.Blogs.Find(id);
            if (dbblog != null)
            {
                DeleteBlogComments(dbblog.Id);
                context.Blogs.Remove(dbblog);
                context.SaveChanges();
            }
            return dbblog;
        }
        public void DeleteBlogComments(int id)
        {
            List<Blog> blogComments = context.Blogs.Where(b => b.ParentId == id&& b.IsBlogComment==1).ToList(); 
            if (blogComments.Count>1)
            {
                foreach ( var blog in blogComments)
                {
                    Blog comment = context.Blogs.Find(blog.Id);
                    if (comment != null)
                    {
                        context.Blogs.Remove(comment);
                        context.SaveChanges();
                    }
                }
            }
        }

        public Blog SaveBlog(Blog blog)
        {
            if (blog.Id == 0)
            {
                context.Blogs.Add(blog);
            }
            else
            {
                Blog dbEntry = context.Blogs.Find(blog.Id);
                if (dbEntry != null)
                {
                    dbEntry.Author = blog.Author;
                    dbEntry.ParentId = blog.ParentId;
                    dbEntry.Content = blog.Content;
                    dbEntry.EntryDate = blog.EntryDate;
                    dbEntry.ImageUrl = blog.ImageUrl;
                    dbEntry.IsBlogComment = blog.IsBlogComment;
                    dbEntry.Title = blog.Title;
                    dbEntry.Popularity = blog.Popularity;
                    dbEntry.AuthorEmail = blog.AuthorEmail;

                }
            }
            context.SaveChanges();
            return blog;
        }

       
        public Blog SaveBlog(Blog blog, bool isComment)
        {
            if (blog.Id == 0)
            {
                if (isComment == true)
                    blog.IsBlogComment = 1;
                context.Blogs.Add(blog);
                Blog parent = context.Blogs.Where(p => p.ParentId == blog.ParentId).FirstOrDefault();
                if (parent != null)
                {
                    parent.Popularity = parent.Popularity + 1;
                    SaveBlog(parent);
                }
            }
            else
            {
                Blog dbEntry = context.Blogs.Find(blog.Id);
                if (dbEntry != null)
                {
                    dbEntry.Author = blog.Author;
                    dbEntry.Content = blog.Content;
                    dbEntry.Title = blog.Title;
                    dbEntry.Popularity = blog.Popularity;
                    dbEntry.AuthorEmail = blog.AuthorEmail;
                }
            }
            context.SaveChanges();
            return blog;
        }
    }
}
