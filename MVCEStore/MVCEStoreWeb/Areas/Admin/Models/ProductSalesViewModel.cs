using MVCEStoreData;

namespace MVCEStoreWeb.Areas.Admin.Models
{
    public class ProductSalesViewModel
    {
        public Product  Product { get; set; }
        public decimal Sales { get; set; }
        public int Quantity { get; set; }
    }
}
