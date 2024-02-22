using System;
using System.Collections.Generic;

namespace Storefront.DATA.EF.Models
{
    public partial class Record
    {
        public Record()
        {
            RecordOrders = new HashSet<RecordOrder>();
        }

        public int RecordId { get; set; }
        public string RecordName { get; set; } = null!;
        public int ArtistId { get; set; }
        public string? Year { get; set; }
        public decimal? Price { get; set; }
        public string? CoverArt { get; set; }
        public string? ExecutiveProducer { get; set; }
        public int StatusId { get; set; }

        public virtual Artist? Artist { get; set; } = null!;
        public virtual ProductStatus? Status { get; set; } = null!;
        public virtual ICollection<RecordOrder> RecordOrders { get; set; }
       
    }
}
