using Storefront.DATA.EF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storefront.DATA.EF.Metadata
{
    internal class Metadata
    {

        public class ArtistMetadata
        {
            [Required(ErrorMessage = "Artist Name is required")]
            [StringLength(100)]
            [Display(Name = "Name")]
            public string ArtistName { get; set; } = null!;
            public int ArtistId { get; set; } //Primary Key/No Metadata
            [StringLength(50)]
            [Display(Name = "City")]
            [Required]
            public string City { get; set; } = null!;
            [StringLength(2)]
            [Display(Name = "State")]
            public string? State { get; set; }
            [StringLength(50)]
            [Display(Name = "Country")]
            [Required]
            public string Country { get; set; } = null!;
            [StringLength(12)]
            [Display(Name = "Years Active")]
            public string? YearsActive { get; set; }
            [Range(0, 100, ErrorMessage = "Number of albums must be between 0 and 100")]
            [Display(Name = "Number of Albums")]
            [Required]
            public int NumberOfAlbums { get; set; }
            [StringLength(150)]
            [Display(Name = "Members")]
            public string? Members { get; set; }
            public int? GenreId { get; set; } //Foreign Key/No Metadata
            [StringLength(50)]
            [Display(Name = "Genre")]
            public string? Genre { get; set; }

            public int RecordID { get; set; }


        }

      

            public class CustomerDataMetadata
            {

                public string CustomerId { get; set; } = null!; //Primary Key/No Metadata
                [StringLength(50)]
                [Display(Name = "First Name")]
                [Required]
                public string? FirstName { get; set; }
                [StringLength(50)]
                [Display(Name = "Last Name")]
                [Required]
                public string LastName { get; set; } = null!;
                public int OrderId { get; set; } //Foreign Key/No Metadata
                [StringLength(50)]
                [Display(Name = "City")]
                [Required]
                public string? CustomerCity { get; set; }
                [StringLength(2)]
                [Display(Name = "State")]
                public string? CustomerState { get; set; }
                [StringLength(5)]
                [DataType(DataType.PostalCode)]
                public string? CustomerZip { get; set; }
                [StringLength(50)]
                [Display(Name = "Country")]
                [Required]
                public string CustomerCountry { get; set; } = null!;
            [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
            public string? Phone { get; set; }

        }

            public class OrderMetadata
        {

            public int OrderId { get; set; } //Primary Key/No Metadata
            [StringLength(25)]
            [Display(Name = "Fulfillment Status")]
            
            public string FulfillmentStatus { get; set; } = null!;
            public string CustomerId { get; set; }  //Foreign Key/No Metadata
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]//0:d => MM/dd/yyyy
            [Display(Name = "Order Date")]
            [Required]
            public DateTime? OrderDate { get; set; }
            [InverseProperty("Order")]
            public virtual ICollection<RecordOrder> RecordOrders { get; set; }
            [ForeignKey(nameof(CustomerId))]
            public virtual CustomerData? Customer { get; set; } = null!;
        }

        public class ProductStatusMetadata
        {

            public int StatusId { get; set; } //Primary Key/No Metadata
            [Required(ErrorMessage = "Record Name is required")]
            [StringLength(100)]
            [Display(Name = "Name")]
            public string? RecordName { get; set; }
            [StringLength(50)]
            [Display(Name = "Product Status")]
            [Required]
            public string? ProductStatus1 { get; set; }

        }

        public class RecordMetadata
        {

            public int RecordId { get; set; } //Primary Key/No Metadata
            [StringLength(100)]
            [Display(Name = "Record Name")]
            [Required(ErrorMessage = "Record name is required")]
            public string RecordName { get; set; } = null!;
            public int ArtistId { get; set; } //Foreign Key/No Metadata
            [StringLength(50)]
            [Display(Name = "Genre")]
            [Required]
            public string Genre { get; set; } = null!;
            [StringLength(4)]
            [Display(Name = "Release Year")]
            public string? Year { get; set; }
            [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
            [Display(Name = "Price")]
            [Range(0, (double)decimal.MaxValue)]
            [Required]
            public decimal Price { get; set; }
            [StringLength(100)]
            [Required(ErrorMessage = "Must input file path for album art")]
            public string? CoverArt { get; set; }
            [StringLength(100)]
            [Display(Name = "Executive Producer(s)")]
            public string? ExecutiveProducer { get; set; }

            public int? StatusId { get; set; } //Foreign Key/No Metadata

            public int? RecordOrderId { get; set; } //Foreign Key/No Metadata

        }

        public class GenreMetadata
        {

            public int GenreId { get; set; } //Primary Key/No Metadata
            [StringLength(50)]
            [Display(Name = "Genre")]
            public string GenreName { get; set; }
            [StringLength(250)]
            [Display(Name = "Genre Description")]
            public string GenreDescription { get; set; }

        }

        public class RecordOrderMetadata
        {

            public int RecordOrderId { get; set; } //Primary Key/No Metadata
            public int? OrderId { get; set; } //Foreign Key/No Metadata
            public int? RecordId { get; set; }

        }


    }
}