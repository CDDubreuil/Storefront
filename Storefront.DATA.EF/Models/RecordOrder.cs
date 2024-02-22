using System;
using System.Collections.Generic;

namespace Storefront.DATA.EF.Models
{
    public partial class RecordOrder
    {
        public int RecordOrderId { get; set; }
        public int OrderId { get; set; }
        public int RecordId { get; set; }

        public virtual Order? Order { get; set; } = null!;
        public virtual Record? Record { get; set; } = null!;
    }
}
