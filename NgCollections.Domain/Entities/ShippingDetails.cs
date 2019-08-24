using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgCollections.Domain.Entities
{
    public class ShippingDetails
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter a name")]
        [Display(Name="Full name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter a email")]
        [Display(Name = "Email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter your phone number")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Customer Phone", Prompt = "(123) 8033208157")]
        public string PhoneNumber { get; set; }
        public Cart Cart { get; set; }
    }
}
