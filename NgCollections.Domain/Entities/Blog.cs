using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgCollections.Domain.Entities
{
  public  class Blog
    {
        public int Id { get; set; }
        public DateTime EntryDate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public string AuthorEmail { get; set; }
        public int ParentId { get; set; }
        public int IsBlogComment { get; set; }
        public string ImageUrl { get; set; }

        public string NumberOfComments { get; set; }
        public int Popularity { get; set; }

    }
}
