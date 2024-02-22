using Storefront.DATA.EF.Models;
namespace Storefront.Models
{
    public class CartItemViewModel
    {
        public int Qty { get; set; } 
        public Record Record { get; set; }
        public CartItemViewModel(int qty, Record record)
        {
            Qty = qty;
            Record = record;
        }
    }
}
