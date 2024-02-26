using System.ComponentModel.DataAnnotations;

namespace Storefront.Models
{
    public class CheckoutViewModel
    {
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;

        [StringLength(50)]
        [Display(Name = "City")]
        public string CustomerCity { get; set; } = null!;

        [StringLength(2, MinimumLength = 2)]
        [Display(Name = "State")]
        public string CustomerState { get; set; } = null!;

        [StringLength(5, MinimumLength = 5)]
        [Display(Name = "Zip")]
        public string CustomerZip { get; set; } = null!;
		public string FullName { get; internal set; }
	}
}
