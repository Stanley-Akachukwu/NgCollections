using NgCollections.Domain.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NgCollections.WebUI.Models
{
    public class ProductVM
    {
        private EFDbContext context = new EFDbContext();

        [HiddenInput(DisplayValue = false)]
        public int ProductID { get; set; }
        [Required(ErrorMessage = "Please enter a product name")]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Please enter a description")]
        public string Description { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive price")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Please specify a category")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public string FrontImageUrl { get; set; }
        public string BackImageUrl { get; set; }
        public int? NavId { get; set; }
        public List<SelectListItem> ProductNumberSizes { get; set; }
        [Display(Name = "Size")]

        public int ProductNumberSize { get; set; }
        [Display(Name = "Available Sizes")]
        public string AvailableSizes { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public ProductVM()
        {
            Categories = GetCategories();
            ProductNumberSizes = GetProductNumberSizes();
        }
       
        public List<SelectListItem> GetProductNumberSizes()
        {
            List<SelectListItem> sizes = new List<SelectListItem>();
            for(int i =25; i<=60; i++)
            {
                sizes.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            return sizes;
        }
        
        public List<SelectListItem> GetCategories()
        {
            var dbCates = context.Categories.ToList();
            List<SelectListItem> cats = new List<SelectListItem>();
            foreach (var item in dbCates)
            {
                cats.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }
            return cats;
        }
    }
}