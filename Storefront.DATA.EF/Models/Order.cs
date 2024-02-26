using System;
using System.Collections.Generic;

namespace Storefront.DATA.EF.Models
{
    public partial class Order
    {
        public Order()
        {
            RecordOrders = new HashSet<RecordOrder>();
        }

        public int OrderId { get; set; }
        public string FulfillmentStatus { get; set; } = null!;
        public string CustomerId { get; set; }
        public DateTime? OrderDate { get; set; }

       public virtual CustomerData? Customer { get; set; } 
        public virtual ICollection<RecordOrder> RecordOrders { get; set; }

       
    }
}
