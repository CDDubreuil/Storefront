using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Storefront.DATA.EF.Models
{
    public partial class CustomerData
    {
        public string? CustomerId { get; set; } //Primary Key/No Metadata   
        public string? FirstName { get; set; }
        public string LastName { get; set; } = null!;
        public string? CustomerCity { get; set; }
        public string? CustomerState { get; set; }
        public string? CustomerZip { get; set; }
        public string CustomerCountry { get; set; } = null!;
        public string Phone { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<Order>? Orders { get; set; }
    }
}