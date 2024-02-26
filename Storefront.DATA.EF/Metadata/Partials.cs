using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Storefront.DATA.EF.Metadata.Metadata;

namespace Storefront.DATA.EF.Models
{
    [ModelMetadataType(typeof(ArtistMetadata))]
    public partial class Artist
    {
        [NotMapped]
        public string SearchString => $"{ArtistName}";

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        [NotMapped]
        [FileExtensions(Extensions = "png,jpeg,jpg,gif", ErrorMessage = ".png, .jpeg, .jpg, .gif")]
        public string? ImageName => ImageFile?.FileName;
    }

    [ModelMetadataType(typeof(CustomerDataMetadata))]
    public partial class CustomerData
    {
        [NotMapped]
        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

    }

    [ModelMetadataType(typeof(OrderMetadata))]
    public partial class Order
    {

    } 
    
    [ModelMetadataType(typeof(ProductStatusMetadata))]
    public partial class ProductStatus
    {

    }
    
    [ModelMetadataType(typeof(RecordMetadata))]
    public partial class Record
    {
        public string SearchString => $"{RecordName} {ArtistId}";
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        [NotMapped]
        [FileExtensions(Extensions = "png,jpeg,jpg,gif", ErrorMessage = ".png, .jpeg, .jpg, .gif")]
        public string? ImageName => ImageFile?.FileName;
    }

    [ModelMetadataType(typeof(GenreMetadata))]
    public partial class Genre
    {

    }

    [ModelMetadataType(typeof(RecordOrderMetadata))]
    public partial class RecordOrder
    {

    } 
    
   
    

}