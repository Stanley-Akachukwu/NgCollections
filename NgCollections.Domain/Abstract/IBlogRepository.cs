using NgCollections.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgCollections.Domain.Abstract
{
   public interface IBlogRepository
    {
        Blog SaveBlog(Blog blog);
        IEnumerable<Blog> Blogs { get; }
        Blog DeleteBlog(int Id);
        Blog SaveBlog(Blog blog, bool isComment);
    }
}
